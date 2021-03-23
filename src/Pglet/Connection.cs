using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Pglet
{
    public class Connection
    {
        private const int CONNECTION_TIMEOUT = 5000;
        private const int MAX_WRITE_BUFFER = 65535;
        private const string ERROR_RESULT = "error";

        string _pipeId;
        NamedPipeClientStream _commandPipe;
        NamedPipeClientStream _eventPipe;
        StreamReader _commandPipeReader;
        StreamWriter _commandPipeWriter;
        StreamReader _eventPipeReader;

        readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        Event _lastEvent;
        AutoResetEvent _resetEvent = new AutoResetEvent(false);

        Action<Event> _onEvent;

        public string PipeId
        {
            get { return _pipeId; }
        }

        public Action<Event> OnEvent
        {
            get { return _onEvent; }
            set { _onEvent = value; }
        }

        public Connection(string pipeId)
        {
            _pipeId = pipeId;
        }

        public async Task OpenAsync(CancellationToken cancellationToken)
        {
            _commandPipe = new NamedPipeClientStream(_pipeId);
            _eventPipe = new NamedPipeClientStream($"{_pipeId}.events");

            await _commandPipe.ConnectAsync(CONNECTION_TIMEOUT, cancellationToken);
            await _eventPipe.ConnectAsync(CONNECTION_TIMEOUT, cancellationToken);

            _commandPipeReader = new StreamReader(_commandPipe);
            _commandPipeWriter = new StreamWriter(_commandPipe, new UTF8Encoding(false, true), MAX_WRITE_BUFFER);
            _commandPipeWriter.AutoFlush = true;
            _eventPipeReader = new StreamReader(_eventPipe);

            var t = Task.Run(() => EventLoop());
        }

        public string SendBatch(IEnumerable<string> commands)
        {
            return SendBatchAsync(commands).GetAwaiter().GetResult();
        }

        public Task<string> SendBatchAsync(IEnumerable<string> commands)
        {
            return SendBatchAsync(commands, CancellationToken.None);
        }

        public async Task<string> SendBatchAsync(IEnumerable<string> commands, CancellationToken cancellationToken)
        {
            await _semaphore.WaitAsync(cancellationToken);

            try
            {
                await SendAsyncInternal("begin");

                foreach (var command in commands)
                {
                    await SendAsyncInternal(command);
                }

                return await SendAsyncInternal("end");
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public string Send(string commandText)
        {
            return SendAsync(commandText).GetAwaiter().GetResult();
        }

        public Task<string> SendAsync(string commandText)
        {
            return SendAsync(commandText, CancellationToken.None);
        }

        public async Task<string> SendAsync(string commandText, CancellationToken cancellationToken)
        {
            await _semaphore.WaitAsync(cancellationToken);

            try
            {
                return await SendAsyncInternal(commandText);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private async Task<string> SendAsyncInternal(string commandText)
        {
            bool waitResult = true;
            var match = Regex.Match(commandText, @"(?<commandName>[^\s]+)\s(.*)");
            if (match.Success)
            {
                var commandName = match.Groups["commandName"].Value;
                if (commandName.ToLowerInvariant().EndsWith("f"))
                {
                    waitResult = false;
                }
            }

            await _commandPipeWriter.WriteLineAsync(commandText);

            if (waitResult)
            {
                var result = _commandPipeReader.ReadLine();

                if (result.StartsWith($"{ERROR_RESULT} "))
                {
                    throw new Exception(result.Substring(ERROR_RESULT.Length + 1));
                }
                else
                {
                    var resultMatch = Regex.Match(result, @"(?<lines_count>[\d]+)\s(?<result>.*)");
                    if (resultMatch.Success)
                    {
                        var linesCount = Int32.Parse(resultMatch.Groups["lines_count"].Value);
                        result = resultMatch.Groups["result"].Value;
                        for (int i = 0; i < linesCount; i++)
                        {
                            var line = _commandPipeReader.ReadLine();
                            result += $"\n{line}";
                        }
                    }
                    else
                    {
                        throw new Exception($"Invalid result: {result}");
                    }
                }

                return result;
            }

            return null;
        }

        public void EventLoop()
        {
            while(true)
            {
                foreach(var e in WaitEventsInternal())
                {
                    _onEvent?.Invoke(e);

                    if (e.Target != "page" || e.Name != "change")
                    {
                        _lastEvent = e;
                        _resetEvent.Set();
                    }
                }
            }
        }

        public Event WaitEvent()
        {
            return WaitEvent(CancellationToken.None);
        }

        public Event WaitEvent(CancellationToken cancellationToken)
        {
            _resetEvent.Reset();

            int n = WaitHandle.WaitAny(new[] { _resetEvent, cancellationToken.WaitHandle });
            if (n == 1)
            {
                cancellationToken.ThrowIfCancellationRequested();
            }

            return _lastEvent;
        }

        private IEnumerable<Event> WaitEventsInternal()
        {
            yield return ParseEventLine(_eventPipeReader.ReadLine());
        }

        private Event ParseEventLine(string line)
        {
            var match = Regex.Match(line, @"(?<target>[^\s]+)\s(?<name>[^\s]+)(\s(?<data>.+))*");
            if (match.Success)
            {
                return new Event
                {
                    Target = match.Groups["target"].Value,
                    Name = match.Groups["name"].Value,
                    Data = match.Groups["data"].Value
                };
            }
            else
            {
                throw new Exception($"Invalid event data: {line}");
            }
        }

        public void Close()
        {
            _eventPipeReader.Dispose();
            _commandPipeReader.Dispose();
            _commandPipe.Dispose();
            _eventPipe.Dispose();
        }
    }
}

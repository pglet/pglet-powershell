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
        bool _inBatch;

        Event _lastEvent;
        AutoResetEvent _resetEvent = new AutoResetEvent(false);

        Dictionary<string, Dictionary<string, EventHandler>> _controlEventHandlers = new Dictionary<string, Dictionary<string, EventHandler>>(StringComparer.OrdinalIgnoreCase);

        public string PipeId
        {
            get { return _pipeId; }
        }

        public Connection(string pipeId)
        {
            _pipeId = pipeId;
        }

        public async Task OpenAsync()
        {
            if (!RuntimeInfo.IsLinux && !RuntimeInfo.IsMac)
            {
                _commandPipe = new NamedPipeClientStream(_pipeId);
                _eventPipe = new NamedPipeClientStream($"{_pipeId}.events");

                await _commandPipe.ConnectAsync(CONNECTION_TIMEOUT);
                await _eventPipe.ConnectAsync(CONNECTION_TIMEOUT);

                _commandPipeReader = new StreamReader(_commandPipe);
                _commandPipeWriter = new StreamWriter(_commandPipe, new UTF8Encoding(false, true), MAX_WRITE_BUFFER);
                _commandPipeWriter.AutoFlush = true;
                _eventPipeReader = new StreamReader(_eventPipe);
            }

            //var t = Task.Run(() => EventLoop());
            var t = Task.Factory.StartNew(
                action: () => EventLoop(),
                cancellationToken: CancellationToken.None,
                creationOptions: TaskCreationOptions.LongRunning,
                scheduler: TaskScheduler.Default
            );
        }

        public string SendBatch(IEnumerable<string> commands)
        {
            return SendBatchAsync(commands).GetAwaiter().GetResult();
        }

        public string Send(string commandText)
        {
            return SendAsync(commandText).GetAwaiter().GetResult();
        }

        public async Task<string> SendBatchAsync(IEnumerable<string> commands)
        {
            _inBatch = true;
            await SendAsync("begin");

            foreach(var command in commands)
            {
                await SendAsync(command);
            }

            _inBatch = false;
            return await SendAsync("end");
        }

        public async Task<string> SendAsync(string commandText)
        {
            bool waitResult = true;
            if (_inBatch)
            {
                waitResult = false;
            }
            else
            {
                var match = Regex.Match(commandText, @"(?<commandName>[^\s]+)\s(.*)");
                if (match.Success)
                {
                    var commandName = match.Groups["commandName"].Value;
                    if (commandName.ToLowerInvariant().EndsWith("f"))
                    {
                        waitResult = false;
                    }
                }
            }

            if (RuntimeInfo.IsLinux || RuntimeInfo.IsMac)
            {
                _commandPipeWriter = new StreamWriter(_pipeId);
            }

            await _commandPipeWriter.WriteLineAsync(commandText);

            if (RuntimeInfo.IsLinux || RuntimeInfo.IsMac)
            {
                _commandPipeWriter.Close();
            }

            if (waitResult)
            {
                try
                {
                    if (RuntimeInfo.IsLinux || RuntimeInfo.IsMac)
                    {
                        _commandPipeReader = new StreamReader(_pipeId);
                    }

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
                finally
                {
                    if (RuntimeInfo.IsLinux || RuntimeInfo.IsMac)
                    {
                        _commandPipeReader.Close();
                    }
                }
            }

            return null;
        }

        public void EventLoop()
        {
            while(true)
            {
                _lastEvent = WaitEventInternal();

                // call event handlers
                if (_controlEventHandlers.ContainsKey(_lastEvent.Target))
                {
                    var controlHandlers = _controlEventHandlers[_lastEvent.Target];
                    if (controlHandlers.ContainsKey(_lastEvent.Name))
                    {
                        var t = Task.Run(() => controlHandlers[_lastEvent.Name](_lastEvent));
                    }
                }

                _resetEvent.Set();
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

        private Event WaitEventInternal()
        {
            if (RuntimeInfo.IsLinux || RuntimeInfo.IsMac)
            {
                _eventPipeReader = new StreamReader($"{_pipeId}.events");
            }

            try
            {
                var line = _eventPipeReader.ReadLine();

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
            finally
            {
                if (RuntimeInfo.IsLinux || RuntimeInfo.IsMac)
                {
                    _eventPipeReader.Close();
                }
            }
        }

        internal void AddEventHandler(string controlId, string eventName, EventHandler handler)
        {
            Dictionary<string, EventHandler> controlEvents = null;
            if (_controlEventHandlers.ContainsKey(controlId))
            {
                controlEvents = _controlEventHandlers[controlId];
            }
            else
            {
                controlEvents = new Dictionary<string, EventHandler>();
                _controlEventHandlers[controlId] = controlEvents;
            }
            controlEvents[eventName] = handler;
        }

        internal void RemoveEventHandler(string controlId, string eventName)
        {
            Dictionary<string, EventHandler> controlEvents = null;
            if (_controlEventHandlers.ContainsKey(controlId))
            {
                controlEvents = _controlEventHandlers[controlId];
                controlEvents.Remove(eventName);
            }
        }

        internal void RemoveEventHandlers(string controlId)
        {
            _controlEventHandlers.Remove(controlId);
        }

        public void Close()
        {
            if (!RuntimeInfo.IsLinux && !RuntimeInfo.IsMac)
            {
                _eventPipeReader.Dispose();
                _commandPipeReader.Dispose();
                _commandPipe.Dispose();
                _eventPipe.Dispose();
            }
        }
    }
}

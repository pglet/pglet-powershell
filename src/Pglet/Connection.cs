using System;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Text.RegularExpressions;
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

        public string PipeId
        {
            get { return _pipeId; }
        }

        public Connection(string pipeId)
        {
            _pipeId = pipeId;
        }

        public async Task Open()
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
        }

        public async Task<string> Send(string commandText)
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

            if (RuntimeInfo.IsLinux && RuntimeInfo.IsMac)
            {
                _commandPipeWriter = new StreamWriter(_pipeId);
            }

            await _commandPipeWriter.WriteLineAsync(commandText);

            if (RuntimeInfo.IsLinux && RuntimeInfo.IsMac)
            {
                _commandPipeWriter.Close();
            }

            if (waitResult)
            {
                try
                {
                    if (RuntimeInfo.IsLinux && RuntimeInfo.IsMac)
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
                    if (RuntimeInfo.IsLinux && RuntimeInfo.IsMac)
                    {
                        _commandPipeReader.Close();
                    }
                }
            }

            return null;
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

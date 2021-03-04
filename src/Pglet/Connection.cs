using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading.Tasks;

namespace Pglet
{
    public class Connection
    {
        private const int CONNECTION_TIMEOUT = 5000;
        private const int MAX_WRITE_BUFFER = 65535;

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

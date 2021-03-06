using System;
using System.IO;
using System.Management.Automation;
using System.Threading;
using System.Threading.Tasks;

namespace Pglet.PowerShell
{
    [Cmdlet(VerbsCommunications.Connect, "PgletApp")]
    [OutputType(typeof(Page))]
    public class ConnectPgletAppCommandSync : PSCmdlet
    {
        readonly CancellationTokenSource _cancellationSource = new CancellationTokenSource();

        [Parameter(Mandatory = false, Position = 0, HelpMessage = "The name of Pglet page.")]
        public string Name { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "A handler script block for a new user session.")]
        public ScriptBlock ScriptBlock { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Makes the page available as public at pglet.io service or a self-hosted Pglet server.")]
        public SwitchParameter Web { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Do not open browser window.")]
        public SwitchParameter NoWindow { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Connects to the page on a self-hosted Pglet server.")]
        public string Server { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Authentication token for pglet.io service or a self-hosted Pglet server.")]
        public string Token { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Interval in milliseconds between 'tick' events; disabled if not specified.")]
        public int? Ticker { get; set; }

        protected override void ProcessRecord()
        {
            WriteObject("ProcessRecord!");

            //await Task.Delay(2000);
            //var result = this.SessionState.InvokeCommand.InvokeScript(ScriptBlock.ToString());
            //this.WriteObject(r, false);
            //this.WriteObject(result, false);

            Pglet.App((page) =>
            {
                File.AppendAllText(@"C:\projects\2\sessions.txt", page.Connection.PipeId);
                Task.Delay(30000).Wait();
                //WriteObject($"connection ID: {page.Connection.PipeId}");
                //var result = this.SessionState.InvokeCommand.InvokeScript(ScriptBlock.ToString());
                //this.WriteObject(result, false);
                return Task.CompletedTask;
            },
            cancellationToken: _cancellationSource.Token, name: Name, web: Web.ToBool(), noWindow: NoWindow.ToBool(),
                server: Server, token: Token, ticker: Ticker.HasValue ? Ticker.Value : 0).Wait();
        }

        protected override void StopProcessing()
        {
            _cancellationSource.Cancel();
            base.StopProcessing();
        }
    }
}

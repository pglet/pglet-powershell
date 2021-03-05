using System;
using System.Management.Automation;
using System.Threading;
using System.Threading.Tasks;

namespace Pglet.PowerShell
{
    [Cmdlet(VerbsCommunications.Connect, "PgletApp")]
    [OutputType(typeof(Page))]
    public class ConnectPgletAppCommand : AsyncCmdlet
    {
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

        protected override async Task ProcessRecordAsync(CancellationToken cancellationToken)
        {
            WriteObject("ProcessRecordAsync!");

            await Pglet.App((page) =>
            {
                WriteObject($"connection started: {page.Connection.PipeId}");
                Task.Delay(10000).Wait();
                WriteObject($"connection end: {page.Connection.PipeId}");
                //var result = this.SessionState.InvokeCommand.InvokeScript(ScriptBlock.ToString());
                //this.WriteObject(result, false);
                return Task.CompletedTask;
            },
            cancellationToken: cancellationToken, name: Name, web: Web.ToBool(), noWindow: NoWindow.ToBool(),
                server: Server, token: Token, ticker: Ticker.HasValue ? Ticker.Value : 0);
        }
    }
}

using System;
using System.IO;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Threading;
using System.Threading.Tasks;

namespace Pglet.PowerShell
{
    [Cmdlet(VerbsCommunications.Connect, "PgletApp")]
    public class ConnectPgletAppCommand : PSCmdlet
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

            Pglet.App(async (page) =>
            {
                var runspace = RunspaceFactory.CreateRunspace();
                var ps = System.Management.Automation.PowerShell.Create();
                ps.Runspace = runspace;
                runspace.Open();
                runspace.InitialSessionState.Variables.Add(new SessionStateVariableEntry("PGLET_TEST", "aaa", ""));
                ps.AddScript(ScriptBlock.ToString());
                var handle = ps.BeginInvoke();

                //File.AppendAllText(@"C:\projects\2\sessions.txt", $"start of: {page.Connection.PipeId}\n");
                //await Task.Delay(30000);
                //File.AppendAllText(@"C:\projects\2\sessions.txt", $"end of: {page.Connection.PipeId}\n");
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

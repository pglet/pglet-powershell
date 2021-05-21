using Pglet.PowerShell.Controls;
using System;
using System.Management.Automation;

namespace Pglet.PowerShell
{
    [Cmdlet(VerbsCommunications.Connect, "PgletPage")]
    [OutputType(typeof(Page))]
    public class ConnectPgletPageCommand : PSCmdlet
    {
        [Parameter(Mandatory = false, Position = 0, HelpMessage = "The name of Pglet page.")]
        public string Name { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Run the page on the local instance of Pglet server.")]
        public SwitchParameter Local { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Do not open browser window.")]
        public SwitchParameter NoWindow { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Connects to the page on a self-hosted Pglet server.")]
        public string Server { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Authentication token for pglet.io service or a self-hosted Pglet server.")]
        public string Token { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Interval in milliseconds between 'tick' events; disabled if not specified.")]
        public int? Ticker { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "The list of users and groups allowed to access this page.")]
        public string Permissions { get; set; }

        protected override void ProcessRecord()
        {
            var page = PgletClient.ConnectPage(name: Name, local: Local.ToBool(), noWindow: NoWindow.ToBool(),
                server: Server, token: Token, ticker: Ticker.HasValue ? Ticker.Value : 0, permissions: Permissions,
                createPage: (conn, pageUrl) => new PsPage(conn, pageUrl)).GetAwaiter().GetResult();

            SessionState.PSVariable.Set(new PSVariable(Constants.PGLET_PAGE, page, ScopedItemOptions.Private));
            WriteObject(page);
        }
    }
}

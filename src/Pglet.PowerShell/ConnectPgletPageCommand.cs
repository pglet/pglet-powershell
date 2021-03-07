﻿using System.Management.Automation;
using System.Threading;
using System.Threading.Tasks;

namespace Pglet.PowerShell
{
    [Cmdlet(VerbsCommunications.Connect, "PgletPage")]
    [OutputType(typeof(Page))]
    public class ConnectPgletPageCommand : PSCmdlet
    {
        [Parameter(Mandatory = false, Position = 0, HelpMessage = "The name of Pglet page.")]
        public string Name { get; set; }

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
            var page = Pglet.Page(name: Name, web: Web.ToBool(), noWindow: NoWindow.ToBool(),
                server: Server, token: Token, ticker: Ticker.HasValue ? Ticker.Value : 0).GetAwaiter().GetResult();
            WriteObject(page);
        }
    }
}

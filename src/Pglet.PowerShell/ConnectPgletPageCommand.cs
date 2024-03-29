﻿using Pglet.PowerShell.Controls;
using System.Management.Automation;

namespace Pglet.PowerShell
{
    [Cmdlet(VerbsCommunications.Connect, "PgletPage")]
    [OutputType(typeof(Page))]
    public class ConnectPgletPageCommand : PSCmdlet
    {
        [Parameter(Mandatory = false, Position = 0, HelpMessage = "The name of Pglet page.")]
        public string Name { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Do not open browser window.")]
        public SwitchParameter NoWindow { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Stream web page to a hosted Pglet service.")]
        public SwitchParameter Web { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Connects to the page on a self-hosted Pglet server.")]
        public string Server { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Authentication token for pglet.io service or a self-hosted Pglet server.")]
        public string Token { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "The list of users and groups allowed to access this page.")]
        public string Permissions { get; set; }

        protected override void ProcessRecord()
        {
            var page = PgletClient.ConnectPage(pageName: Name, web: Web.ToBool(), noWindow: NoWindow.ToBool(),
                serverUrl: Server, token: Token, permissions: Permissions,
                createPage: (conn, pageUrl, pageName, sessionId) =>
                    new PsPage(conn, pageUrl, pageName, sessionId, this)).GetAwaiter().GetResult();

            SessionState.PSVariable.Set(new PSVariable(Constants.PGLET_PAGE, page, ScopedItemOptions.Private));
            WriteObject(page);
        }
    }
}

using System;
using System.Management.Automation;

namespace Pglet.PowerShell
{
    [Cmdlet(VerbsCommon.Clear, "PgletPage")]
    public class ClearPgletPageCommand : PSCmdlet
    {
        [Parameter(Mandatory = false, Position = 0, HelpMessage = "Page object to disconnect from.")]
        public Page Page { get; set; }

        protected override void ProcessRecord()
        {
            var page = Page;
            if (page == null)
            {
                page = SessionState.PSVariable.Get(Constants.PGLET_PAGE).Value as Page;
            }

            if (page == null)
            {
                throw new Exception("There are no active Pglet connections.");
            }

            page.Clean();
        }
    }
}

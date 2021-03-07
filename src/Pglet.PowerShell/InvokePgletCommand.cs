using System;
using System.Management.Automation;
using System.Threading;
using System.Threading.Tasks;

namespace Pglet.PowerShell
{
    [Cmdlet(VerbsLifecycle.Invoke, "Pglet")]
    public class InvokePgletCommand : PSCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Page command to invoke.")]
        public string Command { get; set; }

        [Parameter(Mandatory = false, Position = 1, HelpMessage = "Page object to invoke command against.")]
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

            var result = page.Connection.Send(Command);
            if (!String.IsNullOrEmpty(result))
            {
                WriteObject(result);
            }
        }
    }
}

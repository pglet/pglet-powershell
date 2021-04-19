using System;
using System.Management.Automation;
using System.Threading;

namespace Pglet.PowerShell
{
    [Cmdlet(VerbsLifecycle.Wait, "PgletEvent")]
    [OutputType(typeof(Event))]
    public class WaitPgletEventCommand : PSCmdlet
    {
        readonly CancellationTokenSource _cancellationSource = new();

        [Parameter(Mandatory = false, Position = 0, HelpMessage = "Page object to wait for event from.")]
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

            var e = page.Connection.WaitEvent(_cancellationSource.Token);
            WriteObject(e);
        }

        protected override void StopProcessing()
        {
            _cancellationSource.Cancel();
            base.StopProcessing();
        }
    }
}

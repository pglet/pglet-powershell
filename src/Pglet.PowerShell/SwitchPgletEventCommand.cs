using System;
using System.Management.Automation;
using System.Threading;

namespace Pglet.PowerShell
{
    [Cmdlet(VerbsCommon.Switch, "PgletEvent")]
    [OutputType(typeof(Event))]
    public class SwitchPgletEventCommand : PSCmdlet
    {
        readonly CancellationTokenSource _cancellationSource = new CancellationTokenSource();

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

            while(!_cancellationSource.IsCancellationRequested)
            {
                var e = page.Connection.WaitEvent(_cancellationSource.Token);

                var ctl = page.GetControl(e.Target);
                if (ctl == null)
                {
                    continue;
                }

                var eventCtl = ctl as IPsEventTarget;
                if (eventCtl == null)
                {
                    continue;
                }

                var handlerScript = eventCtl.GetEventHandlerScript(e.Name);

                this.InvokeCommand.InvokeScript(this.SessionState, handlerScript, e);

                //WriteObject(e);
            }
        }

        protected override void StopProcessing()
        {
            _cancellationSource.Cancel();
            base.StopProcessing();
        }
    }
}

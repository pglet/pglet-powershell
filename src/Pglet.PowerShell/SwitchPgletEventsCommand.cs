using Pglet.PowerShell.Controls;
using System;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Threading;

namespace Pglet.PowerShell
{
    [Cmdlet(VerbsCommon.Switch, "PgletEvents")]
    [OutputType(typeof(Event))]
    public class SwitchPgletEventsCommand : PSCmdlet
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

            while(!_cancellationSource.IsCancellationRequested)
            {
                var e = page.Connection.WaitEvent(_cancellationSource.Token);

                var ctl = page.GetControl(e.Target);
                if (ctl == null)
                {
                    continue;
                }

                IPsEventControl eventCtl = ctl as IPsEventControl;
                if (eventCtl == null)
                {
                    continue;
                }

                var handlerScript = eventCtl.GetEventHandlerScript(e.Name);
                if (handlerScript == null)
                {
                    continue;
                }

                var script = $"$event = $args[0]\n{handlerScript}";
                var results = this.InvokeCommand.InvokeScript(script, true, PipelineResultTypes.None, null, e);

                foreach (var obj in results)
                {
                    WriteObject(obj);
                }

                if (e.Target == "page" && e.Name == "close")
                {
                    return;
                }
            }
        }

        protected override void StopProcessing()
        {
            _cancellationSource.Cancel();
            base.StopProcessing();
        }
    }
}

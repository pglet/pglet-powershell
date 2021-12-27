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

            while (!_cancellationSource.IsCancellationRequested)
            {
                var e = page.WaitEvent(_cancellationSource.Token);

                IPsEventControl eventCtl = e.Control as IPsEventControl;
                if (eventCtl == null)
                {
                    continue;
                }

                var handlerScript = eventCtl.GetEventHandler(e);
                if (handlerScript.Item1 == null)
                {
                    continue;
                }

                try
                {
                    handlerScript.Item2["e"] = e;
                    var script = "foreach($key in $args[0].keys) {\n" +
                        "Set-Variable -Name $key -Value $args[0][$key]\n" +
                        "}\n" + handlerScript.Item1;

                    var results = this.InvokeCommand.InvokeScript(script, true, PipelineResultTypes.None, null, handlerScript.Item2);

                    foreach (var obj in results)
                    {
                        WriteObject(obj);
                    }
                }
                catch(Exception ex)
                {
                    var msg = $"Event handler error: {ex.Message}";
                    var re = ex as System.Management.Automation.RuntimeException;
                    if (re != null)
                    {
                        msg = re.ErrorRecord.ToString() + re.ErrorRecord.InvocationInfo.PositionMessage;
                    }
                    Console.WriteLine(msg);
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

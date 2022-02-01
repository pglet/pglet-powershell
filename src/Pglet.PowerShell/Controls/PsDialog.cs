using System;
using System.Collections.Generic;
using System.Management.Automation;

namespace Pglet.PowerShell.Controls
{
    public class PsDialog : Dialog, IPsEventControl
    {
        public PSCmdlet Cmdlet { get; set; }

        public Dictionary<string, (ScriptBlock, Dictionary<string, object>)> PsEventHandlers { get; } =
            new Dictionary<string, (ScriptBlock, Dictionary<string, object>)>(StringComparer.OrdinalIgnoreCase);

        public new ScriptBlock OnDismiss
        {
            get
            {
                return PsEventControlHelper.GetEventHandlerScript(this, "dismiss");
            }
            set
            {
                PsEventControlHelper.SetEventHandlerScript(this, "dismiss", value);
            }
        }

        public (ScriptBlock, Dictionary<string, object>) GetEventHandler(ControlEvent e)
        {
            return PsEventControlHelper.GetEventHandler(this, e.Name);
        }
    }
}

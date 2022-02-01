using System;
using System.Collections.Generic;
using System.Management.Automation;

namespace Pglet.PowerShell.Controls
{
    public class PsSpinButton : SpinButton, IPsEventControl
    {
        public PSCmdlet Cmdlet { get; set; }

        public Dictionary<string, (ScriptBlock, Dictionary<string, object>)> PsEventHandlers { get; } =
            new Dictionary<string, (ScriptBlock, Dictionary<string, object>)>(StringComparer.OrdinalIgnoreCase);

        public new ScriptBlock OnChange
        {
            get
            {
                return PsEventControlHelper.GetEventHandlerScript(this, "change");
            }
            set
            {
                PsEventControlHelper.SetEventHandlerScript(this, "change", value);
                SetBoolAttr("onchange", value != null);
            }
        }

        public (ScriptBlock, Dictionary<string, object>) GetEventHandler(ControlEvent e)
        {
            return PsEventControlHelper.GetEventHandler(this, e.Name);
        }
    }
}

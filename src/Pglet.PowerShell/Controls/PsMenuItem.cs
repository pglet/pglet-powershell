using System;
using System.Collections.Generic;
using System.Management.Automation;

namespace Pglet.PowerShell.Controls
{
    public class PsMenuItem : MenuItem, IPsEventControl
    {
        public PSCmdlet Cmdlet { get; set; }

        public Dictionary<string, (ScriptBlock, Dictionary<string, object>)> PsEventHandlers { get; } =
            new Dictionary<string, (ScriptBlock, Dictionary<string, object>)>(StringComparer.OrdinalIgnoreCase);

        public new ScriptBlock OnClick
        {
            get
            {
                return PsEventControlHelper.GetEventHandlerScript(this, "click");
            }
            set
            {
                PsEventControlHelper.SetEventHandlerScript(this, "click", value);
            }
        }

        public (ScriptBlock, Dictionary<string, object>) GetEventHandler(ControlEvent e)
        {
            return PsEventControlHelper.GetEventHandler(this, e.Name);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Management.Automation;

namespace Pglet.PowerShell.Controls
{
    public class PsNav : Nav, IPsEventControl
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
            }
        }

        public new ScriptBlock OnExpand
        {
            get
            {
                return PsEventControlHelper.GetEventHandlerScript(this, "expand");
            }
            set
            {
                PsEventControlHelper.SetEventHandlerScript(this, "expand", value);
            }
        }

        public new ScriptBlock OnCollapse
        {
            get
            {
                return PsEventControlHelper.GetEventHandlerScript(this, "collapse");
            }
            set
            {
                PsEventControlHelper.SetEventHandlerScript(this, "collapse", value);
            }
        }

        public (ScriptBlock, Dictionary<string, object>) GetEventHandler(ControlEvent e)
        {
            return PsEventControlHelper.GetEventHandler(this, e.Name);
        }
    }
}

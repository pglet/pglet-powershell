using System;
using System.Collections.Generic;
using System.Management.Automation;

namespace Pglet.PowerShell.Controls
{
    public class PsGrid : Grid, IPsEventControl
    {
        public PSCmdlet Cmdlet { get; set; }

        public Dictionary<string, (ScriptBlock, Dictionary<string, object>)> PsEventHandlers { get; } =
            new Dictionary<string, (ScriptBlock, Dictionary<string, object>)>(StringComparer.OrdinalIgnoreCase);

        public new ScriptBlock OnSelect
        {
            get
            {
                return PsEventControlHelper.GetEventHandlerScript(this, "select");
            }
            set
            {
                PsEventControlHelper.SetEventHandlerScript(this, "select", value);
            }
        }

        public new ScriptBlock OnItemInvoke
        {
            get
            {
                return PsEventControlHelper.GetEventHandlerScript(this, "itemInvoke");
            }
            set
            {
                PsEventControlHelper.SetEventHandlerScript(this, "itemInvoke", value);
            }
        }

        public (ScriptBlock, Dictionary<string, object>) GetEventHandler(ControlEvent e)
        {
            OnSelectInternal(e);
            return PsEventControlHelper.GetEventHandler(this, e.Name);
        }
    }
}

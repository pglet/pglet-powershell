using Pglet.Controls;
using System;
using System.Collections.Generic;
using System.Management.Automation;

namespace Pglet.PowerShell.Controls
{
    public class PsDatePicker : DatePicker, IPsEventControl
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

        public (ScriptBlock, Dictionary<string, object>) GetEventHandler(ControlEvent e)
        {
            return PsEventControlHelper.GetEventHandler(this, e.Name);
        }
    }
}

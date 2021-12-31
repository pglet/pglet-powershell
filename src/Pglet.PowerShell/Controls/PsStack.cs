using System;
using System.Collections.Generic;
using System.Management.Automation;

namespace Pglet.PowerShell.Controls
{
    public class PsStack : Pglet.Controls.Stack, IPsEventControl
    {
        public PSCmdlet Cmdlet { get; set; }
        
        public Dictionary<string, (ScriptBlock, Dictionary<string, object>)> PsEventHandlers { get; } =
            new Dictionary<string, (ScriptBlock, Dictionary<string, object>)>(StringComparer.OrdinalIgnoreCase);
            
        public new ScriptBlock OnSubmit
        {
            get
            {
                return PsEventControlHelper.GetEventHandlerScript(this, "submit");
            }
            set
            {
                PsEventControlHelper.SetEventHandlerScript(this, "submit", value);
                SetBoolAttr("onsubmit", value != null);
            }
        }

        public (ScriptBlock, Dictionary<string, object>) GetEventHandler(ControlEvent e)
        {
            return PsEventControlHelper.GetEventHandler(this, e.Name);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Management.Automation;

namespace Pglet.PowerShell.Controls
{
    public class PsStack : Pglet.Controls.Stack, IPsEventControl
    {
        readonly Dictionary<string, ScriptBlock> _psEvents = new Dictionary<string, ScriptBlock>(StringComparer.OrdinalIgnoreCase);

        public new ScriptBlock OnSubmit
        {
            get
            {
                return GetEventHandlerScript("submit");
            }
            set
            {
                _psEvents["submit"] = value;
                SetBoolAttr("onsubmit", value != null);
            }
        }

        public ScriptBlock GetEventHandlerScript(ControlEvent e)
        {
            return GetEventHandlerScript(e.Name);
        }

        private ScriptBlock GetEventHandlerScript(string eventName)
        {
            return _psEvents.ContainsKey(eventName) ? _psEvents[eventName] : null;
        }
    }
}

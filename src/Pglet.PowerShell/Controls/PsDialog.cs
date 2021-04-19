using Pglet.Controls;
using System;
using System.Collections.Generic;
using System.Management.Automation;

namespace Pglet.PowerShell.Controls
{
    public class PsDialog : Dialog, IPsEventControl
    {
        readonly Dictionary<string, ScriptBlock> _psEvents = new Dictionary<string, ScriptBlock>(StringComparer.OrdinalIgnoreCase);

        public new ScriptBlock OnDismiss
        {
            get
            {
                return GetEventHandlerScript("dismiss");
            }
            set
            {
                _psEvents["dismiss"] = value;
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

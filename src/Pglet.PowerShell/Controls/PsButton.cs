using Pglet.Controls;
using System.Collections.Generic;
using System.Management.Automation;

namespace Pglet.PowerShell.Controls
{
    public class PsButton : Button, IPsEventTarget
    {
        Dictionary<string, ScriptBlock> _psEvents = new Dictionary<string, ScriptBlock>();

        public new ScriptBlock OnClick
        {
            get
            {
                return _psEvents.ContainsKey("click") ? _psEvents["click"] : null;
            }
            set
            {
                _psEvents["click"] = value;
            }
        }

        public ScriptBlock GetEventHandlerScript(string eventName)
        {
            return _psEvents.ContainsKey(eventName) ? _psEvents[eventName] : null;
        }
    }
}

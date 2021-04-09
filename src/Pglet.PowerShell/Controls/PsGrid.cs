using Pglet.Controls;
using System;
using System.Collections.Generic;
using System.Management.Automation;

namespace Pglet.PowerShell.Controls
{
    public class PsGrid : Grid, IPsEventControl
    {
        readonly Dictionary<string, ScriptBlock> _psEvents = new Dictionary<string, ScriptBlock>(StringComparer.OrdinalIgnoreCase);

        public new ScriptBlock OnSelect
        {
            get
            {
                return GetEventHandlerScript("select");
            }
            set
            {
                _psEvents["select"] = value;
            }
        }

        public new ScriptBlock OnItemInvoke
        {
            get
            {
                return GetEventHandlerScript("itemInvoke");
            }
            set
            {
                _psEvents["itemInvoke"] = value;
            }
        }

        public ScriptBlock GetEventHandlerScript(string eventName)
        {
            return _psEvents.ContainsKey(eventName) ? _psEvents[eventName] : null;
        }
    }
}

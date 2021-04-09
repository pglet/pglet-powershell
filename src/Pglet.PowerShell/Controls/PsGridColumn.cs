using Pglet.Controls;
using System;
using System.Collections.Generic;
using System.Management.Automation;

namespace Pglet.PowerShell.Controls
{
    public class PsGridColumn : GridColumn, IPsEventControl
    {
        readonly Dictionary<string, ScriptBlock> _psEvents = new Dictionary<string, ScriptBlock>(StringComparer.OrdinalIgnoreCase);

        public new ScriptBlock OnClick
        {
            get
            {
                return GetEventHandlerScript("click");
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

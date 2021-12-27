using Pglet.Controls;
using System;
using System.Collections.Generic;
using System.Management.Automation;

namespace Pglet.PowerShell.Controls
{
    public class PsNav : Nav, IPsEventControl
    {
        public PSCmdlet Cmdlet { get; set; }
        public Dictionary<string, object> PSVariables { get; set;}
        
        readonly Dictionary<string, ScriptBlock> _psEvents = new Dictionary<string, ScriptBlock>(StringComparer.OrdinalIgnoreCase);

        public new ScriptBlock OnChange
        {
            get
            {
                return GetEventHandlerScript("change");
            }
            set
            {
                _psEvents["change"] = value;
            }
        }

        public new ScriptBlock OnExpand
        {
            get
            {
                return GetEventHandlerScript("expand");
            }
            set
            {
                _psEvents["expand"] = value;
            }
        }

        public new ScriptBlock OnCollapse
        {
            get
            {
                return GetEventHandlerScript("collapse");
            }
            set
            {
                _psEvents["collapse"] = value;
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

using Pglet.Controls;
using System;
using System.Collections.Generic;
using System.Management.Automation;

namespace Pglet.PowerShell.Controls
{
    public class PsGrid : Grid, IPsEventControl
    {
        public PSCmdlet Cmdlet { get; set; }
        public Dictionary<string, object> PSVariables { get; set;}
        
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

        public ScriptBlock GetEventHandlerScript(ControlEvent e)
        {
            OnSelectInternal(e);
            return GetEventHandlerScript(e.Name);
        }

        private ScriptBlock GetEventHandlerScript(string eventName)
        {
            return _psEvents.ContainsKey(eventName) ? _psEvents[eventName] : null;
        }
    }
}

using Pglet.Controls;
using System;
using System.Collections.Generic;
using System.Management.Automation;

namespace Pglet.PowerShell.Controls
{
    public class PsSearchBox : SearchBox, IPsEventControl
    {
        public PSCmdlet Cmdlet { get; set; }
        public Dictionary<string, object> PSVariables { get; set;}
        
        readonly Dictionary<string, ScriptBlock> _psEvents = new Dictionary<string, ScriptBlock>(StringComparer.OrdinalIgnoreCase);

        public new ScriptBlock OnSearch
        {
            get
            {
                return GetEventHandlerScript("search");
            }
            set
            {
                _psEvents["search"] = value;
            }
        }

        public new ScriptBlock OnClear
        {
            get
            {
                return GetEventHandlerScript("clear");
            }
            set
            {
                _psEvents["clear"] = value;
            }
        }

        public new ScriptBlock OnChange
        {
            get
            {
                return GetEventHandlerScript("change");
            }
            set
            {
                _psEvents["change"] = value;
                SetBoolAttr("onchange", value != null);
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

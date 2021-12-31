using Pglet.Controls;
using System;
using System.Collections.Generic;
using System.Management.Automation;

namespace Pglet.PowerShell.Controls
{
    public class PsSearchBox : SearchBox, IPsEventControl
    {
        public PSCmdlet Cmdlet { get; set; }
        
        public Dictionary<string, (ScriptBlock, Dictionary<string, object>)> PsEventHandlers { get; } =
            new Dictionary<string, (ScriptBlock, Dictionary<string, object>)>(StringComparer.OrdinalIgnoreCase);
            
        public new ScriptBlock OnSearch
        {
            get
            {
                return PsEventControlHelper.GetEventHandlerScript(this, "search");
            }
            set
            {
                PsEventControlHelper.SetEventHandlerScript(this, "search", value);
            }
        }

        public new ScriptBlock OnClear
        {
            get
            {
                return PsEventControlHelper.GetEventHandlerScript(this, "clear");
            }
            set
            {
                PsEventControlHelper.SetEventHandlerScript(this, "clear", value);
            }
        }

        public new ScriptBlock OnChange
        {
            get
            {
                return PsEventControlHelper.GetEventHandlerScript(this, "change");
            }
            set
            {
                PsEventControlHelper.SetEventHandlerScript(this, "change", value);
                SetBoolAttr("onchange", value != null);
            }
        }

        public (ScriptBlock, Dictionary<string, object>) GetEventHandler(ControlEvent e)
        {
            return PsEventControlHelper.GetEventHandler(this, e.Name);
        }
    }
}

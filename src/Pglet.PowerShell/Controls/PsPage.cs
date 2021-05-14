using System;
using System.Collections.Generic;
using System.Management.Automation;

namespace Pglet.PowerShell.Controls
{
    public class PsPage : Page, IPsEventControl
    {
        readonly Dictionary<string, ScriptBlock> _psEvents = new Dictionary<string, ScriptBlock>(StringComparer.OrdinalIgnoreCase);

        public PsPage(Connection conn, string pageUrl) : base(conn, pageUrl)
        {
        }

        public new ScriptBlock OnClose
        {
            get
            {
                return GetEventHandlerScript("close");
            }
            set
            {
                _psEvents["close"] = value;
            }
        }

        public new ScriptBlock OnSignin
        {
            get
            {
                return GetEventHandlerScript("signin");
            }
            set
            {
                _psEvents["signin"] = value;
            }
        }

        public new ScriptBlock OnDismissSignin
        {
            get
            {
                return GetEventHandlerScript("dismissSignin");
            }
            set
            {
                _psEvents["dismissSignin"] = value;
            }
        }

        public new ScriptBlock OnHashChange
        {
            get
            {
                return GetEventHandlerScript("hashChange");
            }
            set
            {
                _psEvents["hashChange"] = value;
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

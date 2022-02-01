using System;
using System.Collections.Generic;
using System.Management.Automation;

namespace Pglet.PowerShell.Controls
{
    public class PsPage : Page, IPsEventControl
    {
        public PSCmdlet Cmdlet { get; set; }

        public Dictionary<string, (ScriptBlock, Dictionary<string, object>)> PsEventHandlers { get; } =
            new Dictionary<string, (ScriptBlock, Dictionary<string, object>)>(StringComparer.OrdinalIgnoreCase);

        public PsPage(Protocol.Connection conn, string pageUrl, string pageName, string sessionId, PSCmdlet cmdlet) : base(conn, pageUrl, pageName, sessionId)
        {
            this.Cmdlet = cmdlet;
        }

        public new ScriptBlock OnClose
        {
            get
            {
                return PsEventControlHelper.GetEventHandlerScript(this, "close");
            }
            set
            {
                PsEventControlHelper.SetEventHandlerScript(this, "close", value);
            }
        }

        public new ScriptBlock OnSignin
        {
            get
            {
                return PsEventControlHelper.GetEventHandlerScript(this, "signin");
            }
            set
            {
                PsEventControlHelper.SetEventHandlerScript(this, "signin", value);
            }
        }

        public new ScriptBlock OnDismissSignin
        {
            get
            {
                return PsEventControlHelper.GetEventHandlerScript(this, "dismissSignin");
            }
            set
            {
                PsEventControlHelper.SetEventHandlerScript(this, "dismissSignin", value);
            }
        }

        public new ScriptBlock OnSignout
        {
            get
            {
                return PsEventControlHelper.GetEventHandlerScript(this, "signout");
            }
            set
            {
                PsEventControlHelper.SetEventHandlerScript(this, "signout", value);
            }
        }

        public new ScriptBlock OnHashChange
        {
            get
            {
                return PsEventControlHelper.GetEventHandlerScript(this, "hashChange");
            }
            set
            {
                PsEventControlHelper.SetEventHandlerScript(this, "hashChange", value);
            }
        }

        public new ScriptBlock OnResize
        {
            get
            {
                return PsEventControlHelper.GetEventHandlerScript(this, "resize");
            }
            set
            {
                PsEventControlHelper.SetEventHandlerScript(this, "resize", value);
            }
        }

        public (ScriptBlock, Dictionary<string, object>) GetEventHandler(ControlEvent e)
        {
            return PsEventControlHelper.GetEventHandler(this, e.Name);
        }
    }
}

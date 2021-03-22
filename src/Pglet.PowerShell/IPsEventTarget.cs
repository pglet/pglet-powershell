using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Text;

namespace Pglet.PowerShell
{
    public interface IPsEventTarget
    {
        ScriptBlock GetEventHandlerScript(string eventName);
    }
}

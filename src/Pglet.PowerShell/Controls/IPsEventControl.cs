using System;
using System.Collections.Generic;
using System.Management.Automation;

namespace Pglet.PowerShell.Controls
{
    public interface IPsEventControl
    {
        PSCmdlet Cmdlet { get; set; }
        Dictionary<string, (ScriptBlock, Dictionary<string, object>)> PsEventHandlers { get; }
        (ScriptBlock, Dictionary<string, object>) GetEventHandler(ControlEvent e);
    }
}

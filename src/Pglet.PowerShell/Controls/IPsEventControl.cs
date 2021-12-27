using System.Collections.Generic;
using System.Management.Automation;

namespace Pglet.PowerShell.Controls
{
    public interface IPsEventControl
    {
        PSCmdlet Cmdlet { get; set; }
        Dictionary<string, object> PSVariables { get; set;}
        ScriptBlock GetEventHandlerScript(ControlEvent e);
    }
}

using System.Management.Automation;

namespace Pglet.PowerShell.Controls
{
    public interface IPsEventControl
    {
        ScriptBlock GetEventHandlerScript(ControlEvent e);
    }
}

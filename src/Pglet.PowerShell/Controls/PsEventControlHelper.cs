using System.Linq;
using System.Collections.Generic;
using System.Management.Automation;

namespace Pglet.PowerShell.Controls
{
    public class PsEventControlHelper
    {
        public static void SetEventHandlerScript(IPsEventControl control, string eventName, ScriptBlock handler)
        {
            control.PsEventHandlers[eventName] = (handler, PsEventControlHelper.CaptureLocalVariables(control.Cmdlet));
        }

        public static ScriptBlock GetEventHandlerScript(IPsEventControl control, string eventName)
        {
            return control.PsEventHandlers.ContainsKey(eventName) ? control.PsEventHandlers[eventName].Item1 : null;
        }

        public static (ScriptBlock, Dictionary<string, object>) GetEventHandler(IPsEventControl control, string eventName)
        {
            return control.PsEventHandlers.ContainsKey(eventName) ? control.PsEventHandlers[eventName] : (null, null);
        }        

        private static Dictionary<string, object> CaptureLocalVariables(PSCmdlet cmdlet)
        {
            var result = new Dictionary<string, object>();

            if (cmdlet != null)
            {
                var globalVars = cmdlet.InvokeCommand.InvokeScript("Get-Variable -Scope Global")
                    .Select(v => v.BaseObject as PSVariable).Where(v => v != null);

                var allVars = cmdlet.InvokeCommand.InvokeScript("Get-Variable")
                    .Select(v => v.BaseObject as PSVariable).Where(v => v != null);

                foreach(var v in allVars)
                {
                    if (!globalVars.Any(gv => gv.Name.Equals(v.Name, System.StringComparison.OrdinalIgnoreCase)))
                    {
                        result[v.Name] = v.Value;
                    }
                }  
            }
            return result;
        }
    }    
}
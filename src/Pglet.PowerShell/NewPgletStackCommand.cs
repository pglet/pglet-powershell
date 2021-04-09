using Pglet.Controls;
using System.Management.Automation;

namespace Pglet.PowerShell
{
    [Cmdlet(VerbsCommon.New, "PgletStack")]
    [OutputType(typeof(Stack))]
    public class NewPgletStackCommand : NewControlCmdletBase
    {
        [Parameter(Mandatory = false)]
        public Control[] Controls { get; set; }

        protected override void ProcessRecord()
        {
            var stack = new Stack {};
            SetControlProps(stack);

            foreach (var control in Controls)
            {
                stack.Controls.Add(control);
            }

            WriteObject(stack);
        }
    }
}

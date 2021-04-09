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
            var ctl = new Stack { };
            SetControlProps(ctl);

            if (Controls != null)
            {
                foreach (var control in Controls)
                {
                    ctl.Controls.Add(control);
                }
            }

            WriteObject(ctl);
        }
    }
}

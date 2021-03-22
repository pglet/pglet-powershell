using Pglet.Controls;
using Pglet.PowerShell.Controls;
using System.Management.Automation;

namespace Pglet.PowerShell.Controls
{
    [Cmdlet(VerbsCommon.New, "PgletStack")]
    [OutputType(typeof(Page))]
    public class NewPgletStackCommand : PSCmdlet
    {
        [Parameter(Mandatory = false)]
        public string Id { get; set; }

        [Parameter(Mandatory = false)]
        public Control[] Controls { get; set; }

        protected override void ProcessRecord()
        {
            var ctl = new Stack();
            ctl.Id = Id;
            
            foreach(var control in Controls)
            {
                ctl.Controls.Add(control);
            }

            WriteObject(ctl);
        }
    }
}

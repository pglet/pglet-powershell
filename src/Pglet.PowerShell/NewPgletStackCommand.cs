using Pglet.Controls;
using System.Management.Automation;

namespace Pglet.PowerShell
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
            var ctl = new Stack
            {
                Id = Id
            };

            foreach (var control in Controls)
            {
                ctl.Controls.Add(control);
            }

            WriteObject(ctl);
        }
    }
}

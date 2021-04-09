using Pglet.Controls;
using Pglet.PowerShell.Controls;
using System.Management.Automation;

namespace Pglet.PowerShell
{
    [Cmdlet(VerbsCommon.New, "PgletTab")]
    [OutputType(typeof(Tab))]
    public class NewPgletTabCommand : NewControlCmdletBase
    {
        [Parameter(Mandatory = false)]
        public string Key { get; set; }

        [Parameter(Mandatory = false)]
        public string Text { get; set; }

        [Parameter(Mandatory = false)]
        public string Icon { get; set; }

        [Parameter(Mandatory = false)]
        public string Count { get; set; }

        [Parameter(Mandatory = false)]
        public Control[] Controls { get; set; }

        protected override void ProcessRecord()
        {
            var ctl = new Tab
            {
                Key = Key,
                Text = Text,
                Icon = Icon,
                Count = Count
            };

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

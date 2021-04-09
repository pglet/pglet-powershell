using Pglet.Controls;
using System.Management.Automation;

namespace Pglet.PowerShell
{
    [Cmdlet(VerbsCommon.New, "PgletChoiceGroupOption")]
    [OutputType(typeof(ChoiceGroupOption))]
    public class NewPgletChoiceGroupOptionCommand : NewControlCmdletBase
    {
        [Parameter(Mandatory = false)]
        public string Key { get; set; }

        [Parameter(Mandatory = false)]
        public string Text { get; set; }

        [Parameter(Mandatory = false)]
        public string Icon { get; set; }

        [Parameter(Mandatory = false)]
        public string IconColor { get; set; }

        protected override void ProcessRecord()
        {
            var ctl = new ChoiceGroupOption
            {
                Key = Key,
                Text = Text,
                Icon = Icon,
                IconColor = IconColor
            };

            SetControlProps(ctl);

            WriteObject(ctl);
        }
    }
}

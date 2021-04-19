using Pglet.Controls;
using System.Management.Automation;

namespace Pglet.PowerShell
{
    [Cmdlet(VerbsCommon.New, "PgletDropdownOption")]
    [OutputType(typeof(DropdownOption))]
    public class NewPgletDropdownOptionCommand : NewControlCmdletBase
    {
        [Parameter(Mandatory = false)]
        public string Key { get; set; }

        [Parameter(Mandatory = false)]
        public string Text { get; set; }

        protected override void ProcessRecord()
        {
            var ctl = new DropdownOption
            {
                Key = Key,
                Text = Text
            };

            SetControlProps(ctl);

            WriteObject(ctl);
        }
    }
}

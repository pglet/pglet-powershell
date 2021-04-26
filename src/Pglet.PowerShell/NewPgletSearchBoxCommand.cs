using Pglet.Controls;
using Pglet.PowerShell.Controls;
using System.Management.Automation;

namespace Pglet.PowerShell
{
    [Cmdlet(VerbsCommon.New, "PgletSearchBox")]
    [OutputType(typeof(PsSearchBox))]
    public class NewPgletSearchBoxCommand : NewControlCmdletBase
    {
        [Parameter(Mandatory = false)]
        public string Value { get; set; }

        [Parameter(Mandatory = false)]
        public string Placeholder { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter Underlined { get; set; }

        [Parameter(Mandatory = false)]
        public string Icon { get; set; }

        [Parameter(Mandatory = false)]
        public string IconColor { get; set; }

        [Parameter(Mandatory = false)]
        public ScriptBlock OnSearch { get; set; }

        [Parameter(Mandatory = false)]
        public ScriptBlock OnClear { get; set; }

        [Parameter(Mandatory = false)]
        public ScriptBlock OnChange { get; set; }

        protected override void ProcessRecord()
        {
            var ctl = new PsSearchBox
            {
                Value = Value,
                Placeholder = Placeholder,
                Icon = Icon,
                IconColor = IconColor,
                OnSearch = OnSearch,
                OnClear = OnClear,
                OnChange = OnChange
            };

            if (Underlined.IsPresent)
            {
                ctl.Underlined = Underlined.ToBool();
            }

            SetControlProps(ctl);

            WriteObject(ctl);
        }
    }
}

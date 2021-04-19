using Pglet.Controls;
using Pglet.PowerShell.Controls;
using System.Management.Automation;

namespace Pglet.PowerShell
{
    [Cmdlet(VerbsCommon.New, "PgletDropdown")]
    [OutputType(typeof(PsDropdown))]
    public class NewPgletDropdownCommand : NewControlCmdletBase
    {
        [Parameter(Mandatory = false, Position = 0)]
        public string Label { get; set; }

        [Parameter(Mandatory = false)]
        public string Value { get; set; }

        [Parameter(Mandatory = false)]
        public string Placeholder { get; set; }

        [Parameter(Mandatory = false)]
        public string ErrorMessage { get; set; }

        [Parameter(Mandatory = false)]
        public ScriptBlock OnChange { get; set; }

        [Parameter(Mandatory = false)]
        public DropdownOption[] Options { get; set; }

        protected override void ProcessRecord()
        {
            var ctl = new PsDropdown
            {
                Label = Label,
                Value = Value,
                Placeholder = Placeholder,
                ErrorMessage = ErrorMessage,
                OnChange = OnChange
            };

            SetControlProps(ctl);

            if (Options != null)
            {
                foreach (var option in Options)
                {
                    ctl.Options.Add(option);
                }
            }

            WriteObject(ctl);
        }
    }
}

using Pglet.Controls;
using Pglet.PowerShell.Controls;
using System;
using System.Management.Automation;

namespace Pglet.PowerShell
{
    [Cmdlet(VerbsCommon.New, "PgletDatePicker")]
    [OutputType(typeof(PsDatePicker))]
    public class NewPgletDatePickerCommand : NewControlCmdletBase
    {
        [Parameter(Mandatory = false, Position = 0)]
        public string Label { get; set; }

        [Parameter(Mandatory = false)]
        public DateTime? Value { get; set; }

        [Parameter(Mandatory = false)]
        public string Placeholder { get; set; }

        [Parameter(Mandatory = false)]
        public string ErrorMessage { get; set; }

        [Parameter(Mandatory = false)]
        public string Description { get; set; }

        [Parameter(Mandatory = false)]
        public string Prefix { get; set; }

        [Parameter(Mandatory = false)]
        public string Suffix { get; set; }

        [Parameter(Mandatory = false)]
        public string Icon { get; set; }

        [Parameter(Mandatory = false)]
        public string IconColor { get; set; }

        [Parameter(Mandatory = false)]
        public TextBoxAlign? Align { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter AllowTextInput { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter Required { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter Underlined { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter Borderless { get; set; }

        [Parameter(Mandatory = false)]
        public ScriptBlock OnChange { get; set; }

        protected override void ProcessRecord()
        {
            var control = new PsDatePicker();

            SetControlProps(control);

            control.Label = Label;
            control.Value = Value;
            control.Placeholder = Placeholder;
            control.OnChange = OnChange;

            if (AllowTextInput.IsPresent)
            {
                control.AllowTextInput = AllowTextInput.ToBool();
            }

            if (Required.IsPresent)
            {
                control.Required = Required.ToBool();
            }

            if (Underlined.IsPresent)
            {
                control.Underlined = Underlined.ToBool();
            }

            if (Borderless.IsPresent)
            {
                control.Borderless = Borderless.ToBool();
            }

            WriteObject(control);
        }
    }
}

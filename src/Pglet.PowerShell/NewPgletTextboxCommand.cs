using Pglet.PowerShell.Controls;
using System.Management.Automation;

namespace Pglet.PowerShell
{
    [Cmdlet(VerbsCommon.New, "PgletTextBox")]
    [OutputType(typeof(PsTextBox))]
    public class NewPgletTextBoxCommand : NewControlCmdletBase
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
        public SwitchParameter Multiline { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter Password { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter Required { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter ReadOnly { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter AutoAdjustHeight { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter Underlined { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter Borderless { get; set; }

        [Parameter(Mandatory = false)]
        public ScriptBlock OnChange { get; set; }

        protected override void ProcessRecord()
        {
            var control = new PsTextBox();

            SetControlProps(control);

            control.Label = Label;
            control.Value = Value;
            control.Placeholder = Placeholder;
            control.ErrorMessage = ErrorMessage;
            control.Description = Description;
            control.Prefix = Prefix;
            control.Suffix = Suffix;
            control.Icon = Icon;
            control.IconColor = IconColor;
            control.OnChange = OnChange;

            if (Align.HasValue)
            {
                control.Align = Align.Value;
            }

            if (Multiline.IsPresent)
            {
                control.Multiline = Multiline.ToBool();
            }

            if (Password.IsPresent)
            {
                control.Password = Password.ToBool();
            }

            if (Required.IsPresent)
            {
                control.Required = Required.ToBool();
            }

            if (ReadOnly.IsPresent)
            {
                control.ReadOnly = ReadOnly.ToBool();
            }

            if (AutoAdjustHeight.IsPresent)
            {
                control.AutoAdjustHeight = AutoAdjustHeight.ToBool();
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

using Pglet.Controls;
using Pglet.PowerShell.Controls;
using System.Management.Automation;

namespace Pglet.PowerShell
{
    [Cmdlet(VerbsCommon.New, "PgletTextbox")]
    [OutputType(typeof(PsTextBox))]
    public class NewPgletTextboxCommand : NewControlCmdletBase
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
        public TextBoxAlign? Align { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter Multiline { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter Password { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter Required { get; set; }

        [Parameter(Mandatory = false)]
        public ScriptBlock OnChange { get; set; }

        protected override void ProcessRecord()
        {
            var control = new PsTextBox
            {
                Label = Label,
                Value = Value,
                Placeholder = Placeholder,
                ErrorMessage = ErrorMessage,
                Description = Description,
                OnChange = OnChange
            };

            SetControlProps(control);

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

            WriteObject(control);
        }
    }
}

using Pglet.Controls;
using Pglet.PowerShell.Controls;
using System.Management.Automation;

namespace Pglet.PowerShell
{
    [Cmdlet(VerbsCommon.New, "PgletToggle")]
    [OutputType(typeof(PsToggle))]
    public class NewPgletToggleCommand : NewControlCmdletBase
    {
        [Parameter(Mandatory = false, Position = 0)]
        public string Label { get; set; }

        [Parameter(Mandatory = false)]
        public bool Value { get; set; }

        [Parameter(Mandatory = false)]
        public string ValueField { get; set; }

        [Parameter(Mandatory = false)]
        public string OnText { get; set; }

        [Parameter(Mandatory = false)]
        public string OffText { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter Inline { get; set; }

        [Parameter(Mandatory = false)]
        public ScriptBlock OnChange { get; set; }

        protected override void ProcessRecord()
        {
            var control = new PsToggle
            {
                Label = Label,
                Value = Value,
                ValueField = ValueField,
                OnText = OnText,
                OffText = OffText,
                OnChange = OnChange
            };

            SetControlProps(control);

            if (Inline.IsPresent)
            {
                control.Inline = Inline.ToBool();
            }

            WriteObject(control);
        }
    }
}

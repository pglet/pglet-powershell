using Pglet.PowerShell.Controls;
using System.Management.Automation;

namespace Pglet.PowerShell
{
    [Cmdlet(VerbsCommon.New, "PgletSpinButton")]
    [OutputType(typeof(PsSpinButton))]
    public class NewPgletSpinButtonCommand : NewControlCmdletBase
    {
        [Parameter(Mandatory = false, Position = 0)]
        public string Label { get; set; }

        [Parameter(Mandatory = false)]
        public SpinButtonLabelPosition? LabelPosition { get; set; }

        [Parameter(Mandatory = false)]
        public float? Value { get; set; }

        [Parameter(Mandatory = false)]
        public float? Min { get; set; }

        [Parameter(Mandatory = false)]
        public float? Max { get; set; }

        [Parameter(Mandatory = false)]
        public float? Step { get; set; }

        [Parameter(Mandatory = false)]
        public string ValueField { get; set; }

        [Parameter(Mandatory = false)]
        public string Icon { get; set; }

        [Parameter(Mandatory = false)]
        public ScriptBlock OnChange { get; set; }

        protected override void ProcessRecord()
        {
            var control = new PsSpinButton();

            SetControlProps(control);

            control.Label = Label;
            control.ValueField = ValueField;
            control.Icon = Icon;
            control.OnChange = OnChange;

            if (LabelPosition.HasValue)
            {
                control.LabelPosition = LabelPosition.Value;
            }

            if (Value.HasValue)
            {
                control.Value = Value.Value;
            }

            if (Min.HasValue)
            {
                control.Min = Min.Value;
            }

            if (Max.HasValue)
            {
                control.Max = Max.Value;
            }

            if (Step.HasValue)
            {
                control.Step = Step.Value;
            }

            WriteObject(control);
        }
    }
}

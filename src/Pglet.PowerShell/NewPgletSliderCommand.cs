using Pglet.PowerShell.Controls;
using System.Management.Automation;

namespace Pglet.PowerShell
{
    [Cmdlet(VerbsCommon.New, "PgletSlider")]
    [OutputType(typeof(PsSlider))]
    public class NewPgletSliderCommand : NewControlCmdletBase
    {
        [Parameter(Mandatory = false, Position = 0)]
        public string Label { get; set; }

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
        public string ValueFormat { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter ShowValue { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter Vertical { get; set; }

        [Parameter(Mandatory = false)]
        public ScriptBlock OnChange { get; set; }

        protected override void ProcessRecord()
        {
            var control = new PsSlider();

            SetControlProps(control);

            control.Label = Label;
            control.ValueField = ValueField;
            control.ValueFormat = ValueFormat;
            control.OnChange = OnChange;

            if (ShowValue.IsPresent)
            {
                control.ShowValue = ShowValue.ToBool();
            }

            if (Vertical.IsPresent)
            {
                control.Vertical = Vertical.ToBool();
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

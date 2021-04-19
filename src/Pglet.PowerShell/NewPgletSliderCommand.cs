using Pglet.Controls;
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
        public float Value { get; set; }

        [Parameter(Mandatory = false)]
        public float Min { get; set; }

        [Parameter(Mandatory = false)]
        public float Max { get; set; }

        [Parameter(Mandatory = false)]
        public float Step { get; set; }

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
            var control = new PsSlider
            {
                Label = Label,
                Value = Value,
                ValueField = ValueField,
                ValueFormat = ValueFormat,
                Min = Min,
                Max = Max,
                Step = Step,
                OnChange = OnChange
            };

            SetControlProps(control);

            if (ShowValue.IsPresent)
            {
                control.ShowValue = ShowValue.ToBool();
            }

            if (Vertical.IsPresent)
            {
                control.Vertical = Vertical.ToBool();
            }

            WriteObject(control);
        }
    }
}

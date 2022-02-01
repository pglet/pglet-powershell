using System.Management.Automation;

namespace Pglet.PowerShell
{
    [Cmdlet(VerbsCommon.New, "PgletSpinner")]
    [OutputType(typeof(Spinner))]
    public class NewPgletSpinnerCommand : NewControlCmdletBase
    {
        [Parameter(Mandatory = false, Position = 0)]
        public string Label { get; set; }

        [Parameter(Mandatory = false)]
        public SpinnerSize? Size { get; set; }

        [Parameter(Mandatory = false)]
        public SpinnerLabelPosition? LabelPosition { get; set; }

        protected override void ProcessRecord()
        {
            var control = new Spinner
            {
                Label = Label
            };

            SetControlProps(control);

            if (Size.HasValue)
            {
                control.Size = Size.Value;
            }

            if (LabelPosition.HasValue)
            {
                control.LabelPosition = LabelPosition.Value;
            }

            WriteObject(control);
        }
    }
}

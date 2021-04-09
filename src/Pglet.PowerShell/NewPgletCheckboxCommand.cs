using Pglet.Controls;
using Pglet.PowerShell.Controls;
using System.Management.Automation;

namespace Pglet.PowerShell
{
    [Cmdlet(VerbsCommon.New, "PgletCheckbox")]
    [OutputType(typeof(PsCheckbox))]
    public class NewPgletCheckboxCommand : NewControlCmdletBase
    {
        [Parameter(Mandatory = false, Position = 0)]
        public string Label { get; set; }

        [Parameter(Mandatory = false)]
        public bool Value { get; set; }

        [Parameter(Mandatory = false)]
        public string ValueField { get; set; }

        [Parameter(Mandatory = false)]
        public BoxSide? BoxSide { get; set; }

        [Parameter(Mandatory = false)]
        public ScriptBlock OnChange { get; set; }

        protected override void ProcessRecord()
        {
            var control = new PsCheckbox
            {
                Label = Label,
                Value = Value,
                ValueField = ValueField,
                OnChange = OnChange
            };

            SetControlProps(control);

            if (BoxSide.HasValue)
            {
                control.BoxSide = BoxSide.Value;
            }

            WriteObject(control);
        }
    }
}

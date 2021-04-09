using Pglet.Controls;
using System.Management.Automation;

namespace Pglet.PowerShell
{
    [Cmdlet(VerbsCommon.New, "PgletProgress")]
    [OutputType(typeof(Progress))]
    public class NewPgletProgressCommand : NewControlCmdletBase
    {
        [Parameter(Mandatory = false)]
        public int? Value { get; set; }

        [Parameter(Mandatory = false)]
        public string Label { get; set; }

        [Parameter(Mandatory = false)]
        public string Description { get; set; }

        protected override void ProcessRecord()
        {
            var control = new Progress
            {
                Label = Label,
                Description = Description
            };

            SetControlProps(control);

            if (Value.HasValue)
            {
                control.Value = Value.Value;
            }

            WriteObject(control);
        }
    }
}

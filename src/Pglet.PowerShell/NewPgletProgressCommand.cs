using Pglet.Controls;
using System.Management.Automation;

namespace Pglet.PowerShell
{
    [Cmdlet(VerbsCommon.New, "PgletProgress")]
    [OutputType(typeof(Progress))]
    public class NewPgletProgressCommand : NewControlCmdletBase
    {
        [Parameter(Mandatory = false, Position = 0)]
        public string Label { get; set; }

        [Parameter(Mandatory = false)]
        public int? Value { get; set; }

        [Parameter(Mandatory = false)]
        public string Description { get; set; }

        protected override void ProcessRecord()
        {
            var control = new Progress
            {
                Label = Label,
                Description = Description,
                Value = Value
            };

            SetControlProps(control);

            WriteObject(control);
        }
    }
}

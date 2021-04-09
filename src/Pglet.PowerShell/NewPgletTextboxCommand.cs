using Pglet.Controls;
using System.Management.Automation;

namespace Pglet.PowerShell
{
    [Cmdlet(VerbsCommon.New, "PgletTextbox")]
    [OutputType(typeof(TextBox))]
    public class NewPgletTextboxCommand : NewControlCmdletBase
    {
        [Parameter(Mandatory = false, Position = 0)]
        public string Label { get; set; }

        [Parameter(Mandatory = false)]
        public string Value { get; set; }

        protected override void ProcessRecord()
        {
            var control = new TextBox
            {
                Label = Label,
                Value = Value
            };

            SetControlProps(control);

            WriteObject(control);
        }
    }
}

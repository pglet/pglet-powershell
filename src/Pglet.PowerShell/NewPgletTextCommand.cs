using Pglet.Controls;
using System.Management.Automation;

namespace Pglet.PowerShell
{
    [Cmdlet(VerbsCommon.New, "PgletText")]
    [OutputType(typeof(Text))]
    public class NewPgletTextCommand : NewControlCmdletBase
    {
        [Parameter(Mandatory = false)]
        public string Value { get; set; }

        protected override void ProcessRecord()
        {
            var control = new Text
            {
                Value = Value
            };

            SetControlProps(control);

            WriteObject(control);
        }
    }
}

using Pglet.Controls;
using System.Management.Automation;

namespace Pglet.PowerShell
{
    [Cmdlet(VerbsCommon.New, "PgletHtml")]
    [OutputType(typeof(Html))]
    public class NewPgletHtmlCommand : NewControlCmdletBase
    {
        [Parameter(Mandatory = false, Position = 0)]
        public string Value { get; set; }

        protected override void ProcessRecord()
        {
            var control = new Html
            {
                Value = Value
            };

            WriteObject(control);
        }
    }
}

using Pglet.Controls;
using System.Management.Automation;

namespace Pglet.PowerShell
{
    [Cmdlet(VerbsCommon.New, "PgletText")]
    [OutputType(typeof(Page))]
    public class NewPgletTextCommand : PSCmdlet
    {
        [Parameter(Mandatory = false)]
        public string Id { get; set; }

        [Parameter(Mandatory = false)]
        public string Value { get; set; }

        protected override void ProcessRecord()
        {
            var ctl = new Text
            {
                Id = Id,
                Value = Value
            };

            WriteObject(ctl);
        }
    }
}

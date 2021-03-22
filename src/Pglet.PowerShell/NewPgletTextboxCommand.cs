using Pglet.Controls;
using System.Management.Automation;

namespace Pglet.PowerShell
{
    [Cmdlet(VerbsCommon.New, "PgletTextbox")]
    [OutputType(typeof(Page))]
    public class NewPgletTextboxCommand : PSCmdlet
    {
        [Parameter(Mandatory = false, Position = 0)]
        public string Label { get; set; }

        [Parameter(Mandatory = false)]
        public string Id { get; set; }

        [Parameter(Mandatory = false)]
        public string Value { get; set; }

        protected override void ProcessRecord()
        {
            var ctl = new Textbox();
            ctl.Label = Label;
            ctl.Id = Id;
            ctl.Value = Value;

            WriteObject(ctl);
        }
    }
}

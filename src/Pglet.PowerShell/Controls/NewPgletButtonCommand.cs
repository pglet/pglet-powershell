using Pglet.PowerShell.Controls;
using System.Management.Automation;

namespace Pglet.PowerShell.Controls
{
    [Cmdlet(VerbsCommon.New, "PgletButton")]
    [OutputType(typeof(Page))]
    public class NewPgletButtonCommand : PSCmdlet
    {
        [Parameter(Mandatory = false)]
        public string Id { get; set; }

        [Parameter(Mandatory = false)]
        public string Text { get; set; }

        [Parameter(Mandatory = false)]
        public ScriptBlock OnClick { get; set; }

        protected override void ProcessRecord()
        {
            var ctl = new PsButton();
            ctl.Id = Id;
            ctl.Text = Text;
            ctl.OnClick = OnClick;

            WriteObject(ctl);
        }
    }
}

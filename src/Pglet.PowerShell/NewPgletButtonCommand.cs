using Pglet.PowerShell.Controls;
using System.Management.Automation;

namespace Pglet.PowerShell
{
    [Cmdlet(VerbsCommon.New, "PgletButton")]
    [OutputType(typeof(Page))]
    public class NewPgletButtonCommand : NewControlCmdletBase
    {
        [Parameter(Mandatory = false)]
        public string Text { get; set; }

        [Parameter(Mandatory = false)]
        public ScriptBlock OnClick { get; set; }

        protected override void ProcessRecord()
        {
            var ctl = new PsButton
            {
                Text = Text,
                OnClick = OnClick
            };

            SetControlProps(ctl);

            WriteObject(ctl);
        }
    }
}

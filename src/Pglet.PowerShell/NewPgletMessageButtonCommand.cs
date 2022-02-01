using System.Management.Automation;

namespace Pglet.PowerShell
{
    [Cmdlet(VerbsCommon.New, "PgletMessageButton")]
    [OutputType(typeof(MessageButton))]
    public class NewPgletMessageButtonCommand : NewControlCmdletBase
    {
        [Parameter(Mandatory = false, Position = 0)]
        public string Text { get; set; }

        [Parameter(Mandatory = false)]
        public string Action { get; set; }

        protected override void ProcessRecord()
        {
            var control = new MessageButton
            {
                Action = Action,
                Text = Text
            };

            SetControlProps(control);

            WriteObject(control);
        }
    }
}

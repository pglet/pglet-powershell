using Pglet.Controls;
using Pglet.PowerShell.Controls;
using System.Management.Automation;

namespace Pglet.PowerShell
{
    [Cmdlet(VerbsCommon.New, "PgletMessage")]
    [OutputType(typeof(PsMessage))]
    public class NewPgletMessageCommand : NewControlCmdletBase
    {
        [Parameter(Mandatory = false, Position = 0)]
        public string Value { get; set; }

        [Parameter(Mandatory = false)]
        public MessageButton[] Buttons { get; set; }

        [Parameter(Mandatory = false)]
        public ScriptBlock OnDismiss { get; set; }

        [Parameter(Mandatory = false)]
        public MessageType? Type { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter Multiline { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter Truncated { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter Dismiss { get; set; }

        protected override void ProcessRecord()
        {
            var ctl = new PsMessage();

            SetControlProps(ctl);

            ctl.OnDismiss = OnDismiss;
            ctl.Value = Value;

            if (Multiline.IsPresent)
            {
                ctl.Multiline = Multiline.ToBool();
            }

            if (Truncated.IsPresent)
            {
                ctl.Truncated = Truncated.ToBool();
            }

            if (Dismiss.IsPresent)
            {
                ctl.Dismiss = Dismiss.ToBool();
            }

            if (Type.HasValue)
            {
                ctl.Type = Type.Value;
            }

            if (Buttons != null)
            {
                foreach (var button in Buttons)
                {
                    ctl.Buttons.Add(button);
                }
            }

            WriteObject(ctl);
        }
    }
}

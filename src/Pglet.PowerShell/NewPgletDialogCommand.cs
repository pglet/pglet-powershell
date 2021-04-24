using Pglet.Controls;
using Pglet.PowerShell.Controls;
using System.Management.Automation;

namespace Pglet.PowerShell
{
    [Cmdlet(VerbsCommon.New, "PgletDialog")]
    [OutputType(typeof(PsDialog))]
    public class NewPgletDialogCommand : NewControlCmdletBase
    {
        [Parameter(Mandatory = false)]
        public Control[] Controls { get; set; }

        [Parameter(Mandatory = false)]
        public Control[] FooterControls { get; set; }

        [Parameter(Mandatory = false)]
        public ScriptBlock OnDismiss { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter Open { get; set; }

        [Parameter(Mandatory = false)]
        public string Title { get; set; }

        [Parameter(Mandatory = false)]
        public string SubText { get; set; }

        [Parameter(Mandatory = false)]
        public DialogType? Type { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter AutoDismiss { get; set; }

        [Parameter(Mandatory = false)]
        public string MaxWidth { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter FixedTop { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter Blocking { get; set; }

        protected override void ProcessRecord()
        {
            var ctl = new PsDialog
            {
                OnDismiss = OnDismiss,
                Title = Title,
                SubText = SubText,
                MaxWidth = MaxWidth
            };

            SetControlProps(ctl);

            if (Open.IsPresent)
            {
                ctl.Open = Open.ToBool();
            }

            if (AutoDismiss.IsPresent)
            {
                ctl.AutoDismiss = AutoDismiss.ToBool();
            }

            if (FixedTop.IsPresent)
            {
                ctl.FixedTop = FixedTop.ToBool();
            }

            if (Blocking.IsPresent)
            {
                ctl.Blocking = Blocking.ToBool();
            }

            if (Type.HasValue)
            {
                ctl.Type = Type.Value;
            }

            if (Controls != null)
            {
                foreach (var control in Controls)
                {
                    ctl.Controls.Add(control);
                }
            }

            if (FooterControls != null)
            {
                foreach (var control in FooterControls)
                {
                    ctl.FooterControls.Add(control);
                }
            }

            WriteObject(ctl);
        }
    }
}

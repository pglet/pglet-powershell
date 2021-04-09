using Pglet.Controls;
using Pglet.PowerShell.Controls;
using System.Management.Automation;

namespace Pglet.PowerShell
{
    [Cmdlet(VerbsCommon.New, "PgletPanel")]
    [OutputType(typeof(PsPanel))]
    public class NewPgletPanelCommand : NewControlCmdletBase
    {
        [Parameter(Mandatory = false)]
        public Control[] Controls { get; set; }

        [Parameter(Mandatory = false)]
        public Control[] FooterControls { get; set; }

        [Parameter(Mandatory = false)]
        public ScriptBlock OnDismiss { get; set; }

        [Parameter(Mandatory = false)]
        public PanelType? Type { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter Open { get; set; }

        [Parameter(Mandatory = false)]
        public string Title { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter AutoDismiss { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter LightDismiss { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter Blocking { get; set; }

        protected override void ProcessRecord()
        {
            var ctl = new PsPanel
            {
                OnDismiss = OnDismiss,
                Title = Title
            };

            SetControlProps(ctl);

            if (Type.HasValue)
            {
                ctl.Type = Type.Value;
            }

            if (Open.IsPresent)
            {
                ctl.Open = Open.ToBool();
            }

            if (AutoDismiss.IsPresent)
            {
                ctl.AutoDismiss = AutoDismiss.ToBool();
            }

            if (LightDismiss.IsPresent)
            {
                ctl.LightDismiss = LightDismiss.ToBool();
            }

            if (Blocking.IsPresent)
            {
                ctl.Blocking = Blocking.ToBool();
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

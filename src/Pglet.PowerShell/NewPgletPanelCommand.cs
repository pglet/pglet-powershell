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
            var control = new PsPanel();

            SetControlProps(control);

            control.OnDismiss = OnDismiss;
            control.Title = Title;

            if (Type.HasValue)
            {
                control.Type = Type.Value;
            }

            if (Open.IsPresent)
            {
                control.Open = Open.ToBool();
            }

            if (AutoDismiss.IsPresent)
            {
                control.AutoDismiss = AutoDismiss.ToBool();
            }

            if (LightDismiss.IsPresent)
            {
                control.LightDismiss = LightDismiss.ToBool();
            }

            if (Blocking.IsPresent)
            {
                control.Blocking = Blocking.ToBool();
            }

            if (Controls != null)
            {
                foreach (var childControl in Controls)
                {
                    control.Controls.Add(childControl);
                }
            }

            if (FooterControls != null)
            {
                foreach (var childControl in FooterControls)
                {
                    control.FooterControls.Add(childControl);
                }
            }

            WriteObject(control);
        }
    }
}

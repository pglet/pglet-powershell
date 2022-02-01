using Pglet.PowerShell.Controls;
using System.Management.Automation;

namespace Pglet.PowerShell
{
    [Cmdlet(VerbsCommon.New, "PgletCallout")]
    [OutputType(typeof(PsCallout))]
    public class NewPgletCalloutCommand : NewControlCmdletBase
    {
        [Parameter(Mandatory = false)]
        public Control[] Controls { get; set; }

        [Parameter(Mandatory = false)]
        public ScriptBlock OnDismiss { get; set; }

        [Parameter(Mandatory = false)]
        public string Target { get; set; }

        [Parameter(Mandatory = false)]
        public CalloutPosition? Position { get; set; }

        [Parameter(Mandatory = false)]
        public int? Gap { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter Beak { get; set; }

        [Parameter(Mandatory = false)]
        public int? BeakWidth { get; set; }

        [Parameter(Mandatory = false)]
        public int? PagePadding { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter Focus { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter Cover { get; set; }

        protected override void ProcessRecord()
        {
            var ctl = new PsCallout();

            SetControlProps(ctl);

            ctl.OnDismiss = OnDismiss;
            ctl.Target = Target;

            if (Position.HasValue)
            {
                ctl.Position = Position.Value;
            }

            if (Gap.HasValue)
            {
                ctl.Gap = Gap.Value;
            }

            if (BeakWidth.HasValue)
            {
                ctl.BeakWidth = BeakWidth.Value;
            }

            if (PagePadding.HasValue)
            {
                ctl.PagePadding = PagePadding.Value;
            }

            if (Beak.IsPresent)
            {
                ctl.Beak = Beak.ToBool();
            }

            if (Focus.IsPresent)
            {
                ctl.Focus = Focus.ToBool();
            }

            if (Cover.IsPresent)
            {
                ctl.Cover = Cover.ToBool();
            }

            if (Controls != null)
            {
                foreach (var control in Controls)
                {
                    ctl.Controls.Add(control);
                }
            }

            WriteObject(ctl);
        }
    }
}

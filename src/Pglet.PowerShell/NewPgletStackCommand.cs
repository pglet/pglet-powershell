using Pglet.Controls;
using Pglet.PowerShell.Controls;
using System.Management.Automation;

namespace Pglet.PowerShell
{
    [Cmdlet(VerbsCommon.New, "PgletStack")]
    [OutputType(typeof(PsStack))]
    public class NewPgletStackCommand : NewControlCmdletBase
    {
        [Parameter(Mandatory = false)]
        public Control[] Controls { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter Horizontal { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter VerticalFill { get; set; }

        [Parameter(Mandatory = false)]
        public Align? HorizontalAlign { get; set; }

        [Parameter(Mandatory = false)]
        public Align? VerticalAlign { get; set; }

        [Parameter(Mandatory = false)]
        public string MinWidth { get; set; }

        [Parameter(Mandatory = false)]
        public string MaxWidth { get; set; }

        [Parameter(Mandatory = false)]
        public string MinHeight { get; set; }

        [Parameter(Mandatory = false)]
        public string MaxHeight { get; set; }

        [Parameter(Mandatory = false)]
        public int? Gap { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter Wrap { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter ScrollX { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter ScrollY { get; set; }

        [Parameter(Mandatory = false)]
        public string BgColor { get; set; }

        [Parameter(Mandatory = false)]
        public string Border { get; set; }

        [Parameter(Mandatory = false)]
        public string BorderRadius { get; set; }

        [Parameter(Mandatory = false)]
        public string BorderLeft { get; set; }

        [Parameter(Mandatory = false)]
        public string BorderRight { get; set; }

        [Parameter(Mandatory = false)]
        public string BorderTop { get; set; }

        [Parameter(Mandatory = false)]
        public string BorderBottom { get; set; }

        [Parameter(Mandatory = false)]
        public ScriptBlock OnSubmit { get; set; }

        protected override void ProcessRecord()
        {
            var ctl = new PsStack
            {
                MinWidth = MinWidth,
                MaxWidth = MaxWidth,
                MinHeight = MinHeight,
                MaxHeight = MaxHeight,
                BgColor = BgColor,
                Border = Border,
                BorderRadius = BorderRadius,
                BorderLeft = BorderLeft,
                BorderRight = BorderRight,
                BorderTop = BorderTop,
                BorderBottom = BorderBottom,
                OnSubmit = OnSubmit
            };

            SetControlProps(ctl);

            if (HorizontalAlign.HasValue)
            {
                ctl.HorizontalAlign = HorizontalAlign.Value;
            }

            if (VerticalAlign.HasValue)
            {
                ctl.VerticalAlign = VerticalAlign.Value;
            }

            if (Gap.HasValue)
            {
                ctl.Gap = Gap.Value;
            }

            if (Horizontal.IsPresent)
            {
                ctl.Horizontal = Horizontal.ToBool();
            }

            if (VerticalFill.IsPresent)
            {
                ctl.VerticalFill = VerticalFill.ToBool();
            }

            if (Wrap.IsPresent)
            {
                ctl.Wrap = Wrap.ToBool();
            }

            if (ScrollX.IsPresent)
            {
                ctl.ScrollX = ScrollX.ToBool();
            }

            if (ScrollY.IsPresent)
            {
                ctl.ScrollY = ScrollY.ToBool();
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

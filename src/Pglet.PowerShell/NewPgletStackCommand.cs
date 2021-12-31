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
            var control = new PsStack();

            SetControlProps(control);

            control.MinWidth = MinWidth;
            control.MaxWidth = MaxWidth;
            control.MinHeight = MinHeight;
            control.MaxHeight = MaxHeight;
            control.BgColor = BgColor;
            control.Border = Border;
            control.BorderRadius = BorderRadius;
            control.BorderLeft = BorderLeft;
            control.BorderRight = BorderRight;
            control.BorderTop = BorderTop;
            control.BorderBottom = BorderBottom;
            control.OnSubmit = OnSubmit;

            if (HorizontalAlign.HasValue)
            {
                control.HorizontalAlign = HorizontalAlign.Value;
            }

            if (VerticalAlign.HasValue)
            {
                control.VerticalAlign = VerticalAlign.Value;
            }

            if (Gap.HasValue)
            {
                control.Gap = Gap.Value;
            }

            if (Horizontal.IsPresent)
            {
                control.Horizontal = Horizontal.ToBool();
            }

            if (VerticalFill.IsPresent)
            {
                control.VerticalFill = VerticalFill.ToBool();
            }

            if (Wrap.IsPresent)
            {
                control.Wrap = Wrap.ToBool();
            }

            if (ScrollX.IsPresent)
            {
                control.ScrollX = ScrollX.ToBool();
            }

            if (ScrollY.IsPresent)
            {
                control.ScrollY = ScrollY.ToBool();
            }

            if (Controls != null)
            {
                foreach (var childControl in Controls)
                {
                    control.Controls.Add(childControl);
                }
            }

            WriteObject(control);
        }
    }
}

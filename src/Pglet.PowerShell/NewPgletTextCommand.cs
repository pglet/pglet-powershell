using System.Management.Automation;

namespace Pglet.PowerShell
{
    [Cmdlet(VerbsCommon.New, "PgletText")]
    [OutputType(typeof(Text))]
    public class NewPgletTextCommand : NewControlCmdletBase
    {
        [Parameter(Mandatory = false, Position = 0)]
        public string Value { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter Markdown { get; set; }

        [Parameter(Mandatory = false)]
        public TextAlign? Align { get; set; }

        [Parameter(Mandatory = false)]
        public TextVerticalAlign? VerticalAlign { get; set; }

        [Parameter(Mandatory = false)]
        public TextSize? Size { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter Bold { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter Italic { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter Pre { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter Nowrap { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter Block { get; set; }

        [Parameter(Mandatory = false)]
        public string Color { get; set; }

        [Parameter(Mandatory = false)]
        public string BgColor { get; set; }

        [Parameter(Mandatory = false)]
        public string Border { get; set; }

        [Parameter(Mandatory = false)]
        public BorderStyle? BorderStyle { get; set; }

        [Parameter(Mandatory = false)]
        public string BorderWidth { get; set; }

        [Parameter(Mandatory = false)]
        public string BorderColor { get; set; }

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

        protected override void ProcessRecord()
        {
            var control = new Text
            {
                Value = Value,
                Color = Color,
                BgColor = BgColor,
                Border = Border,
                BorderWidth = BorderWidth,
                BorderColor = BorderColor,
                BorderRadius = BorderRadius,
                BorderLeft = BorderLeft,
                BorderRight = BorderRight,
                BorderTop = BorderTop,
                BorderBottom = BorderBottom,
            };

            SetControlProps(control);

            if (Markdown.IsPresent)
            {
                control.Markdown = Markdown.ToBool();
            }

            if (Bold.IsPresent)
            {
                control.Bold = Bold.ToBool();
            }

            if (Italic.IsPresent)
            {
                control.Italic = Italic.ToBool();
            }

            if (Pre.IsPresent)
            {
                control.Pre = Pre.ToBool();
            }

            if (Nowrap.IsPresent)
            {
                control.Nowrap = Nowrap.ToBool();
            }

            if (Block.IsPresent)
            {
                control.Block = Block.ToBool();
            }

            if (Align.HasValue)
            {
                control.Align = Align.Value;
            }

            if (VerticalAlign.HasValue)
            {
                control.VerticalAlign = VerticalAlign.Value;
            }

            if (Size.HasValue)
            {
                control.Size = Size.Value;
            }

            if (BorderStyle.HasValue)
            {
                control.BorderStyle = BorderStyle.Value;
            }

            WriteObject(control);
        }
    }
}

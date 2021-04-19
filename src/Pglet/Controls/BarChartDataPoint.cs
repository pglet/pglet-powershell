namespace Pglet.Controls
{
    public class BarChartDataPoint : Control
    {
        protected override string ControlName => "p";

        object _x;
        public object X
        {
            get { return _x; }
            set { SetAttr("x", value); _x = value; }
        }

        object _y;
        public object Y
        {
            get { return _y; }
            set { SetAttr("y", value); _y = value; }
        }

        public string Legend
        {
            get { return GetAttr("legend"); }
            set { SetAttr("legend", value); }
        }

        public string Color
        {
            get { return GetAttr("color"); }
            set { SetAttr("color", value); }
        }

        public string XTooltip
        {
            get { return GetAttr("xTooltip"); }
            set { SetAttr("xTooltip", value); }
        }

        public string YTooltip
        {
            get { return GetAttr("yTooltip"); }
            set { SetAttr("yTooltip", value); }
        }
    }
}

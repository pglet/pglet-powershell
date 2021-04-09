namespace Pglet.Controls
{
    public class PieChartDataPoint : Control
    {
        protected override string ControlName => "p";

        object _value;
        public object Value
        {
            get { return _value; }
            set { SetAttr("value", value); _value = value; }
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

        public string Tooltip
        {
            get { return GetAttr("tooltip"); }
            set { SetAttr("tooltip", value); }
        }
    }
}

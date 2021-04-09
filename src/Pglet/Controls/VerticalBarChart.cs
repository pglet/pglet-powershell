using System.Collections.Generic;

namespace Pglet.Controls
{
    public class VerticalBarChart : Control
    {
        protected override string ControlName => "verticalbarchart";

        readonly VerticalBarChartData _data = new();

        public IList<VerticalBarDataPoint> Points
        {
            get { return _data.Points; }
            set { _data.Points = value; }
        }

        public bool Tooltips
        {
            get { return GetBoolAttr("tooltips"); }
            set { SetBoolAttr("tooltips", value); }
        }

        public bool Legend
        {
            get { return GetBoolAttr("legend"); }
            set { SetBoolAttr("legend", value); }
        }

        public int BarWidth
        {
            get { return GetIntAttr("barWidth"); }
            set { SetIntAttr("barWidth", value); }
        }

        public string[] Colors
        {
            get
            {
                var colors = GetAttr("colors");
                return colors != null ? colors.Split(' ', ',') : null;
            }
            set
            {
                SetAttr("yFormat", string.Join(" ", value));
            }
        }

        object _yMin;
        public object YMin
        {
            get { return _yMin; }
            set { SetAttr("yMin", value); _yMin = value; }
        }

        object _yMax;
        public object YMax
        {
            get { return _yMax; }
            set { SetAttr("yMax", value); _yMax = value; }
        }

        public int YTicks
        {
            get { return GetIntAttr("yTicks"); }
            set { SetIntAttr("yTicks", value); }
        }

        public string YFormat
        {
            get { return GetAttr("yFormat"); }
            set { SetAttr("yFormat", value); }
        }

        public VerticalBarChartXType XType
        {
            get { return GetEnumAttr<VerticalBarChartXType>("xType"); }
            set { SetEnumAttr("xType", value); }
        }

        protected override IEnumerable<Control> GetChildren()
        {
            return new Control[] { _data };
        }
    }
}

using System.Collections.Generic;

namespace Pglet.Controls
{
    public class BarChart : Control
    {
        public class DataPoint: Control
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

        public class ChartData : Control
        {
            IList<DataPoint> _points = new List<DataPoint>();

            protected override string ControlName => "data";

            public IList<DataPoint> Points
            {
                get { return _points; }
                set { _points = value; }
            }

            protected override IEnumerable<Control> GetChildren()
            {
                return _points;
            }
        }

        protected override string ControlName => "barchart";

        readonly ChartData _data = new();

        public IList<DataPoint> Points
        {
            get { return _data.Points; }
            set { _data.Points = value; }
        }

        public BarChartDataMode DataMode
        {
            get { return GetEnumAttr<BarChartDataMode>("dataMode"); }
            set { SetEnumAttr("dataMode", value); }
        }

        public bool Tooltips
        {
            get { return GetBoolAttr("tooltips"); }
            set { SetBoolAttr("tooltips", value); }
        }

        protected override IEnumerable<Control> GetChildren()
        {
            return new Control[] { _data };
        }
    }
}

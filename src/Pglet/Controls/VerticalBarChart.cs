using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Pglet.Controls
{
    public enum VerticalBarChartXType
    {
        [Description("number")]
        Number,

        [Description("string")]
        String
    }

    public class VerticalBarChart : Control
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

        protected override string ControlName => "verticalbarchart";

        readonly ChartData _data = new();

        public IList<DataPoint> Points
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

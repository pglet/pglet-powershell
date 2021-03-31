using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Pglet.Controls
{
    public class PieChart : Control
    {
        public class DataPoint: Control
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

        protected override string ControlName => "piechart";

        readonly ChartData _data = new();

        public IList<DataPoint> Points
        {
            get { return _data.Points; }
            set { _data.Points = value; }
        }

        public bool Legend
        {
            get { return GetBoolAttr("legend"); }
            set { SetBoolAttr("legend", value); }
        }

        public bool Tooltips
        {
            get { return GetBoolAttr("tooltips"); }
            set { SetBoolAttr("tooltips", value); }
        }

        object _innerValue;
        public object InnerValue
        {
            get { return _innerValue; }
            set { SetAttr("innerValue", value); _innerValue = value; }
        }

        public int InnerRadius
        {
            get { return GetIntAttr("innerRadius"); }
            set { SetIntAttr("innerRadius", value); }
        }

        protected override IEnumerable<Control> GetChildren()
        {
            return new Control[] { _data };
        }
    }
}

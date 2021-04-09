using System.Collections.Generic;

namespace Pglet.Controls
{
    public partial class LineChart : Control
    {
        protected override string ControlName => "linechart";

        IList<LineChartData> _lines = new List<LineChartData>();

        public IList<LineChartData> Lines
        {
            get { return _lines; }
            set { _lines = value; }
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

        public int StrokeWidth
        {
            get { return GetIntAttr("strokeWidth"); }
            set { SetIntAttr("strokeWidth", value); }
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

        public LineChartXType XType
        {
            get { return GetEnumAttr<LineChartXType>("xType"); }
            set { SetEnumAttr("xType", value); }
        }

        protected override IEnumerable<Control> GetChildren()
        {
            return _lines;
        }
    }
}

using System.Collections.Generic;

namespace Pglet.Controls
{
    public class LineChartData : Control
    {
        IList<LineChartDataPoint> _points = new List<LineChartDataPoint>();

        protected override string ControlName => "data";

        public IList<LineChartDataPoint> Points
        {
            get { return _points; }
            set { _points = value; }
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

        protected override IEnumerable<Control> GetChildren()
        {
            return _points;
        }
    }
}

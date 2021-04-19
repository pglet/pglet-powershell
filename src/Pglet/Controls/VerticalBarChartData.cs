using System.Collections.Generic;

namespace Pglet.Controls
{
    public class VerticalBarChartData : Control
    {
        IList<VerticalBarChartDataPoint> _points = new List<VerticalBarChartDataPoint>();

        protected override string ControlName => "data";

        public IList<VerticalBarChartDataPoint> Points
        {
            get { return _points; }
            set { _points = value; }
        }

        protected override IEnumerable<Control> GetChildren()
        {
            return _points;
        }
    }
}

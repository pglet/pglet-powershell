using System.Collections.Generic;

namespace Pglet.Controls
{
    public class BarChartData : Control
    {
        IList<BarChartDataPoint> _points = new List<BarChartDataPoint>();

        protected override string ControlName => "data";

        public IList<BarChartDataPoint> Points
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

using System.Collections.Generic;

namespace Pglet.Controls
{
    public class VerticalBarChartData : Control
    {
        IList<VerticalBarDataPoint> _points = new List<VerticalBarDataPoint>();

        protected override string ControlName => "data";

        public IList<VerticalBarDataPoint> Points
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

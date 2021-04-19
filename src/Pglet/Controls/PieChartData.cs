using System.Collections.Generic;

namespace Pglet.Controls
{
    public class PieChartData : Control
    {
        IList<PieChartDataPoint> _points = new List<PieChartDataPoint>();

        protected override string ControlName => "data";

        public IList<PieChartDataPoint> Points
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

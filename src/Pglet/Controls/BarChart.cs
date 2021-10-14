using System.Collections.Generic;

namespace Pglet.Controls
{
    public class BarChart : Control
    {
        protected override string ControlName => "barchart";

        readonly BarChartData _data = new();

        public IList<BarChartDataPoint> Points
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

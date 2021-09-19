using System.Collections.Generic;

namespace Pglet.Controls
{
    public class BarChart : Control
    {
        protected override string ControlName => "barchart";

        readonly BarChartData _data = new();

        public ControlCollection<BarChartDataPoint> Points
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

        internal override void SetChildDataLocks(AsyncReaderWriterLock dataLock)
        {
            _data.SetDataLock(dataLock);
        }

        protected override IEnumerable<Control> GetChildren()
        {
            return new Control[] { _data };
        }
    }
}

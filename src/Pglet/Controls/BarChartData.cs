using System.Collections.Generic;

namespace Pglet.Controls
{
    public class BarChartData : Control
    {
        ControlCollection<BarChartDataPoint> _points = new();

        protected override string ControlName => "data";

        public ControlCollection<BarChartDataPoint> Points
        {
            get
            {
                using (var lck = _dataLock.AcquireReaderLock())
                {
                    return _points;
                }
            }
            set
            {
                using (var lck = _dataLock.AcquireWriterLock())
                {
                    _points.SetDataLock(_dataLock);
                    _points = value;
                }
            }
        }

        internal override void SetChildDataLocks(AsyncReaderWriterLock dataLock)
        {
            _points.SetDataLock(dataLock);
        }

        protected override IEnumerable<Control> GetChildren()
        {
            return _points.GetInternalList();
        }
    }
}

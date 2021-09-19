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
                var dlock = _dataLock;
                dlock.AcquireReaderLock();
                try
                {
                    return _points;
                }
                finally
                {
                    dlock.ReleaseReaderLock();
                }
            }
            set
            {
                var dlock = _dataLock;
                dlock.AcquireWriterLock();
                try
                {
                    _points.SetDataLock(_dataLock);
                    _points = value;
                }
                finally
                {
                    dlock.ReleaseWriterLock();
                }
            }
        }

        internal override void SetChildDataLocks(AsyncReaderWriterLock dataLock)
        {
            _points.SetDataLock(dataLock);
        }

        protected override IEnumerable<Control> GetChildren()
        {
            return _points.GetControls();
        }
    }
}

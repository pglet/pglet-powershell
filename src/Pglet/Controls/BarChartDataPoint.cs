namespace Pglet.Controls
{
    public class BarChartDataPoint : Control
    {
        protected override string ControlName => "p";

        object _x;
        public object X
        {
            get
            {
                var dlock = _dataLock;
                dlock.AcquireReaderLock();
                try
                {
                    return _x;
                }
                finally
                {
                    dlock.ReleaseReaderLock();
                }
            }
            set
            {
                SetAttr("x", value);

                var dlock = _dataLock;
                dlock.AcquireWriterLock();
                try
                {
                    _x = value;
                }
                finally
                {
                    dlock.ReleaseWriterLock();
                }
            }
        }

        object _y;
        public object Y
        {
            get
            {
                var dlock = _dataLock;
                dlock.AcquireReaderLock();
                try
                {
                    return _y;
                }
                finally
                {
                    dlock.ReleaseReaderLock();
                }
            }
            set
            {
                SetAttr("y", value);

                var dlock = _dataLock;
                dlock.AcquireWriterLock();
                try
                {
                    _y = value;
                }
                finally
                {
                    dlock.ReleaseWriterLock();
                }
            }
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

        public string XTooltip
        {
            get { return GetAttr("xTooltip"); }
            set { SetAttr("xTooltip", value); }
        }

        public string YTooltip
        {
            get { return GetAttr("yTooltip"); }
            set { SetAttr("yTooltip", value); }
        }
    }
}

using System.Collections.Generic;

namespace Pglet.Controls
{
    public class Footer : Control
    {
        protected override string ControlName => "footer";

        ControlCollection<Control> _controls = new();
        public ControlCollection<Control> Controls
        {
            get
            {
                var dlock = _dataLock;
                dlock.AcquireReaderLock();
                try
                {
                    return _controls;
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
                    _controls.SetDataLock(_dataLock);
                    _controls = value;
                }
                finally
                {
                    dlock.ReleaseWriterLock();
                }
            }
        }

        internal override void SetChildDataLocks(AsyncReaderWriterLock dataLock)
        {
            _controls.SetDataLock(dataLock);
        }

        protected override IEnumerable<Control> GetChildren()
        {
            return _controls.GetInternalList();
        }
    }
}

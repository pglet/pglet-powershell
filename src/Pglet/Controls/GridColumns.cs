using System.Collections.Generic;

namespace Pglet.Controls
{
    public class GridColumns : Control
    {
        internal ControlCollection<GridColumn> _columns = new();

        protected override string ControlName => "columns";

        public ControlCollection<GridColumn> Columns
        {
            get
            {
                var dlock = _dataLock;
                dlock.AcquireReaderLock();
                try
                {
                    return _columns;
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
                    _columns.SetDataLock(_dataLock);
                    _columns = value;
                }
                finally
                {
                    dlock.ReleaseWriterLock();
                }
            }
        }

        internal override void SetChildDataLocks(AsyncReaderWriterLock dataLock)
        {
            _columns.SetDataLock(dataLock);
        }

        protected override IEnumerable<Control> GetChildren()
        {
            return _columns.GetControls();
        }
    }
}

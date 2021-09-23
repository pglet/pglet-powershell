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
                using (var lck = _dataLock.AcquireReaderLock())
                {
                    return _columns;
                }
            }
            set
            {
                using (var lck = _dataLock.AcquireWriterLock())
                {
                    _columns.SetDataLock(_dataLock);
                    _columns = value;
                }
            }
        }

        internal override void SetChildDataLocks(AsyncReaderWriterLock dataLock)
        {
            _columns.SetDataLock(dataLock);
        }

        protected override IEnumerable<Control> GetChildren()
        {
            return _columns.GetInternalList();
        }
    }
}

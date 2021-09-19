using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Pglet
{
    public class ControlCollection : IList<Control>
    {
        readonly List<Control> _list = new List<Control>();
        protected AsyncReaderWriterLock _dataLock = new AsyncReaderWriterLock();

        internal void SetDataLock(AsyncReaderWriterLock dataLock)
        {
            if (_dataLock != dataLock)
            {
                _dataLock = dataLock;
                foreach (var control in _list)
                {
                    control.SetDataLock(dataLock);
                }
            }
        }

        public Control this[int index] {
            get
            {
                var dlock = _dataLock;
                dlock.AcquireReaderLock();
                try
                {
                    return _list[index];
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
                    value.SetDataLock(_dataLock);
                    _list[index] = value;
                }
                finally
                {
                    dlock.ReleaseWriterLock();
                }
            }
        }

        public int Count
        {
            get
            {
                var dlock = _dataLock;
                dlock.AcquireReaderLock();
                try
                {
                    return _list.Count;
                }
                finally
                {
                    dlock.ReleaseReaderLock();
                }
            }
        }

        public bool IsReadOnly => false;

        public void Add(Control item)
        {
            var dlock = _dataLock;
            dlock.AcquireWriterLock();
            try
            {
                item.SetDataLock(_dataLock);
                _list.Add(item);
            }
            finally
            {
                dlock.ReleaseWriterLock();
            }
        }

        public void AddRange(IEnumerable<Control> items)
        {
            var dlock = _dataLock;
            dlock.AcquireWriterLock();
            try
            {
                foreach(var item in items)
                {
                    item.SetDataLock(_dataLock);
                    _list.Add(item);
                }
            }
            finally
            {
                dlock.ReleaseWriterLock();
            }
        }

        public void Clear()
        {
            var dlock = _dataLock;
            dlock.AcquireWriterLock();
            try
            {
                foreach (var item in _list)
                {
                    item.SetDataLock(new AsyncReaderWriterLock());
                }
                _list.Clear();
            }
            finally
            {
                dlock.ReleaseWriterLock();
            }
        }

        public bool Contains(Control item)
        {
            var dlock = _dataLock;
            dlock.AcquireReaderLock();
            try
            {
                return _list.Contains(item);
            }
            finally
            {
                dlock.ReleaseReaderLock();
            }
        }

        public void CopyTo(Control[] array, int arrayIndex)
        {
            var dlock = _dataLock;
            dlock.AcquireReaderLock();
            try
            {
                _list.CopyTo(array, arrayIndex);
            }
            finally
            {
                dlock.ReleaseReaderLock();
            }
        }

        internal IEnumerable<Control> GetControls()
        {
            return _list;
        }

        public IEnumerator<Control> GetEnumerator()
        {
            var dlock = _dataLock;
            dlock.AcquireReaderLock();
            try
            {
                return _list.GetEnumerator();
            }
            finally
            {
                dlock.ReleaseReaderLock();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            var dlock = _dataLock;
            dlock.AcquireReaderLock();
            try
            {
                return _list.GetEnumerator();
            }
            finally
            {
                dlock.ReleaseReaderLock();
            }
        }

        public int IndexOf(Control item)
        {
            var dlock = _dataLock;
            dlock.AcquireReaderLock();
            try
            {
                return _list.IndexOf(item);
            }
            finally
            {
                dlock.ReleaseReaderLock();
            }
        }

        public void Insert(int index, Control item)
        {
            var dlock = _dataLock;
            dlock.AcquireWriterLock();
            try
            {
                item.SetDataLock(_dataLock);
                _list.Insert(index, item);
            }
            finally
            {
                dlock.ReleaseWriterLock();
            }
        }

        public void InsertRange(int index, IEnumerable<Control> items)
        {
            var dlock = _dataLock;
            dlock.AcquireWriterLock();
            try
            {
                int i = index;
                foreach(var item in items)
                {
                    item.SetDataLock(_dataLock);
                    _list.Insert(i++, item);
                }
            }
            finally
            {
                dlock.ReleaseWriterLock();
            }
        }

        public bool Remove(Control item)
        {
            var dlock = _dataLock;
            dlock.AcquireWriterLock();
            try
            {
                item.SetDataLock(new AsyncReaderWriterLock());
                return _list.Remove(item);
            }
            finally
            {
                dlock.ReleaseWriterLock();
            }
        }

        public void RemoveAt(int index)
        {
            var dlock = _dataLock;
            dlock.AcquireWriterLock();
            try
            {
                _list[index].SetDataLock(new AsyncReaderWriterLock());
                _list.RemoveAt(index);
            }
            finally
            {
                dlock.ReleaseWriterLock();
            }
        }
    }
}

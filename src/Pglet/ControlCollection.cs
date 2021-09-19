using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Pglet
{
    public class ControlCollection<T> : IList<T> where T: Control
    {
        readonly List<T> _list = new List<T>();
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

        public T this[int index] {
            get
            {
                var dlock = _dataLock;
                dlock.AcquireReaderLock();
                try
                {
                    return (T)_list[index];
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

        public void Add(T item)
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

        public void AddRange(IEnumerable<T> items)
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

        public bool Contains(T item)
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

        public void CopyTo(T[] array, int arrayIndex)
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

        internal IList<T> GetInternalList()
        {
            return _list;
        }

        public IEnumerator<T> GetEnumerator()
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

        public int IndexOf(T item)
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

        public void Insert(int index, T item)
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

        public void InsertRange(int index, IEnumerable<T> items)
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

        public bool Remove(T item)
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

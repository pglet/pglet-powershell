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
                using (var lck = _dataLock.AcquireReaderLock())
                {
                    return _list[index];
                }
            }
            set
            {
                using (var lck = _dataLock.AcquireWriterLock())
                {
                    value.SetDataLock(_dataLock);
                    _list[index] = value;
                }
            }
        }

        public int Count
        {
            get
            {
                using (var lck = _dataLock.AcquireReaderLock())
                {
                    return _list.Count;
                }
            }
        }

        public bool IsReadOnly => false;

        public void Add(T item)
        {
            using (var lck = _dataLock.AcquireWriterLock())
            {
                item.SetDataLock(_dataLock);
                _list.Add(item);
            }
        }

        public void AddRange(IEnumerable<T> items)
        {
            using (var lck = _dataLock.AcquireWriterLock())
            {
                foreach (var item in items)
                {
                    item.SetDataLock(_dataLock);
                    _list.Add(item);
                }
            }
        }

        public void Clear()
        {
            using (var lck = _dataLock.AcquireWriterLock())
            {
                foreach (var item in _list)
                {
                    item.SetDataLock(new AsyncReaderWriterLock());
                }
                _list.Clear();
            }
        }

        public bool Contains(T item)
        {
            using (var lck = _dataLock.AcquireReaderLock())
            {
                return _list.Contains(item);
            }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            using (var lck = _dataLock.AcquireReaderLock())
            {
                _list.CopyTo(array, arrayIndex);
            }
        }

        internal IList<T> GetInternalList()
        {
            return _list;
        }

        public IEnumerator<T> GetEnumerator()
        {
            using (var lck = _dataLock.AcquireReaderLock())
            {
                return _list.GetEnumerator();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            using (var lck = _dataLock.AcquireReaderLock())
            {
                return _list.GetEnumerator();
            }
        }

        public int IndexOf(T item)
        {
            using (var lck = _dataLock.AcquireReaderLock())
            {
                return _list.IndexOf(item);
            }
        }

        public void Insert(int index, T item)
        {
            using (var lck = _dataLock.AcquireWriterLock())
            {
                item.SetDataLock(_dataLock);
                _list.Insert(index, item);
            }
        }

        public void InsertRange(int index, IEnumerable<T> items)
        {
            using (var lck = _dataLock.AcquireWriterLock())
            {
                int i = index;
                foreach (var item in items)
                {
                    item.SetDataLock(_dataLock);
                    _list.Insert(i++, item);
                }
            }
        }

        public bool Remove(T item)
        {
            using (var lck = _dataLock.AcquireWriterLock())
            {
                item.SetDataLock(new AsyncReaderWriterLock());
                return _list.Remove(item);
            }
        }

        public void RemoveAt(int index)
        {
            using (var lck = _dataLock.AcquireWriterLock())
            {
                _list[index].SetDataLock(new AsyncReaderWriterLock());
                _list.RemoveAt(index);
            }
        }
    }
}

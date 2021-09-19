using System.Collections.Generic;

namespace Pglet.Controls
{
    public class GridColumn : Control
    {
        protected override string ControlName => "column";

        public string Name
        {
            get { return GetAttr("name"); }
            set { SetAttr("name", value); }
        }

        public string Icon
        {
            get { return GetAttr("icon"); }
            set { SetAttr("icon", value); }
        }

        public bool IconOnly
        {
            get { return GetBoolAttr("iconOnly"); }
            set { SetBoolAttr("iconOnly", value); }
        }

        public string FieldName
        {
            get { return GetAttr("fieldName"); }
            set { SetAttr("fieldName", value); }
        }

        public string Sortable
        {
            get { return GetAttr("sortable"); }
            set { SetAttr("sortable", value); }
        }

        public string SortField
        {
            get { return GetAttr("sortField"); }
            set { SetAttr("sortField", value); }
        }

        public string Sorted
        {
            get { return GetAttr("sorted"); }
            set { SetAttr("sorted", value); }
        }

        public bool Resizable
        {
            get { return GetBoolAttr("resizable"); }
            set { SetBoolAttr("resizable", value); }
        }

        public int MinWidth
        {
            get { return GetIntAttr("minWidth"); }
            set { SetIntAttr("minWidth", value); }
        }

        public int MaxWidth
        {
            get { return GetIntAttr("maxWidth"); }
            set { SetIntAttr("maxWidth", value); }
        }

        public EventHandler OnClick
        {
            get { return GetEventHandler("click"); }
            set { SetEventHandler("click", value); }
        }

        ControlCollection<Control> _templateControls = new();
        public ControlCollection<Control> TemplateControls
        {
            get
            {
                var dlock = _dataLock;
                dlock.AcquireReaderLock();
                try
                {
                    return _templateControls;
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
                    _templateControls.SetDataLock(_dataLock);
                    _templateControls = value;
                }
                finally
                {
                    dlock.ReleaseWriterLock();
                }
            }
        }

        internal override void SetChildDataLocks(AsyncReaderWriterLock dataLock)
        {
            _templateControls.SetDataLock(dataLock);
        }

        protected override IEnumerable<Control> GetChildren()
        {
            return _templateControls.GetControls();
        }
    }
}

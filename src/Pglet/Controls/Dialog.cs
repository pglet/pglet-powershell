using System.Collections.Generic;

namespace Pglet.Controls
{
    public class Dialog : Control
    {
        protected override string ControlName => "dialog";

        Footer _footer = new Footer();
        public ControlCollection<Control> FooterControls
        {
            get { return _footer.Controls; }
            set { _footer.Controls = value; }
        }

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

        public bool Open
        {
            get { return GetBoolAttr("open"); }
            set { SetBoolAttr("open", value); }
        }

        public string Title
        {
            get { return GetAttr("title"); }
            set { SetAttr("title", value); }
        }

        public string SubText
        {
            get { return GetAttr("subText"); }
            set { SetAttr("subText", value); }
        }

        public DialogType Type
        {
            get { return GetEnumAttr<DialogType>("type"); }
            set { SetEnumAttr("type", value); }
        }

        public bool AutoDismiss
        {
            get { return GetBoolAttr("autoDismiss"); }
            set { SetBoolAttr("autoDismiss", value); }
        }

        public string MaxWidth
        {
            get { return GetAttr("maxWidth"); }
            set { SetAttr("maxWidth", value); }
        }

        public bool FixedTop
        {
            get { return GetBoolAttr("fixedTop"); }
            set { SetBoolAttr("fixedTop", value); }
        }

        public bool Blocking
        {
            get { return GetBoolAttr("blocking"); }
            set { SetBoolAttr("blocking", value); }
        }

        public EventHandler OnDismiss
        {
            get { return GetEventHandler("dismiss"); }
            set { SetEventHandler("dismiss", value); }
        }

        internal override void SetChildDataLocks(AsyncReaderWriterLock dataLock)
        {
            _footer.SetDataLock(dataLock);
        }

        protected override IEnumerable<Control> GetChildren()
        {
            var controls = new List<Control>();
            controls.AddRange(_controls.GetInternalList());
            controls.Add(_footer);
            return controls;
        }
    }
}

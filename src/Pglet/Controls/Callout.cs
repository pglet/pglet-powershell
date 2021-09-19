using System.Collections.Generic;

namespace Pglet.Controls
{
    public class Callout : Control
    {
        protected override string ControlName => "callout";

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

        public EventHandler OnDismiss
        {
            get { return GetEventHandler("dismiss"); }
            set { SetEventHandler("dismiss", value); }
        }

        public string Target
        {
            get { return GetAttr("target"); }
            set { SetAttr("target", value); }
        }

        public CalloutPosition Position
        {
            get { return GetEnumAttr<CalloutPosition>("position"); }
            set { SetEnumAttr("position", value); }
        }

        public int Gap
        {
            get { return GetIntAttr("gap"); }
            set { SetIntAttr("gap", value); }
        }

        public bool Beak
        {
            get { return GetBoolAttr("beak"); }
            set { SetBoolAttr("beak", value); }
        }

        public int BeakWidth
        {
            get { return GetIntAttr("beakWidth"); }
            set { SetIntAttr("beakWidth", value); }
        }

        public int PagePadding
        {
            get { return GetIntAttr("pagePadding"); }
            set { SetIntAttr("pagePadding", value); }
        }

        public bool Focus
        {
            get { return GetBoolAttr("focus"); }
            set { SetBoolAttr("focus", value); }
        }

        public bool Cover
        {
            get { return GetBoolAttr("cover"); }
            set { SetBoolAttr("cover", value); }
        }

        internal override void SetChildDataLocks(AsyncReaderWriterLock dataLock)
        {
            _controls.SetDataLock(dataLock);
        }

        protected override IEnumerable<Control> GetChildren()
        {
            return _controls.GetControls();
        }
    }
}

using System.Collections.Generic;

namespace Pglet.Controls
{
    public class Dropdown : Control
    {
        protected override string ControlName => "dropdown";

        ControlCollection<DropdownOption> _options = new();
        public ControlCollection<DropdownOption> Options
        {
            get
            {
                var dlock = _dataLock;
                dlock.AcquireReaderLock();
                try
                {
                    return _options;
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
                    _options.SetDataLock(_dataLock);
                    _options = value;
                }
                finally
                {
                    dlock.ReleaseWriterLock();
                }
            }
        }

        public string Value
        {
            get { return GetAttr("value"); }
            set { SetAttr("value", value); }
        }

        public string Label
        {
            get { return GetAttr("label"); }
            set { SetAttr("label", value); }
        }

        public string Placeholder
        {
            get { return GetAttr("placeholder"); }
            set { SetAttr("placeholder", value); }
        }

        public string ErrorMessage
        {
            get { return GetAttr("errorMessage"); }
            set { SetAttr("errorMessage", value); }
        }

        public EventHandler OnChange
        {
            get { return GetEventHandler("change"); }
            set { SetEventHandler("change", value); }
        }

        internal override void SetChildDataLocks(AsyncReaderWriterLock dataLock)
        {
            _options.SetDataLock(dataLock);
        }

        protected override IEnumerable<Control> GetChildren()
        {
            return _options.GetInternalList();
        }
    }
}

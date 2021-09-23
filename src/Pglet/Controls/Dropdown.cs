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
                using (var lck = _dataLock.AcquireReaderLock())
                {
                    return _options;
                }
            }
            set
            {
                using (var lck = _dataLock.AcquireWriterLock())
                {
                    _options.SetDataLock(_dataLock);
                    _options = value;
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

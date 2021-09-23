using System.Collections.Generic;

namespace Pglet.Controls
{
    public class ChoiceGroup : Control
    {
        protected override string ControlName => "choicegroup";

        ControlCollection<ChoiceGroupOption> _options = new();
        public ControlCollection<ChoiceGroupOption> Options
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

        public string Label
        {
            get { return GetAttr("label"); }
            set { SetAttr("label", value); }
        }

        public string Value
        {
            get { return GetAttr("value"); }
            set { SetAttr("value", value); }
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

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

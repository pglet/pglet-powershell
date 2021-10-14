using System.Collections.Generic;

namespace Pglet.Controls
{
    public class ChoiceGroup : Control
    {
        protected override string ControlName => "choicegroup";

        IList<ChoiceGroupOption> _options = new List<ChoiceGroupOption>();

        public IList<ChoiceGroupOption> Options
        {
            get { return _options; }
            set { _options = value; }
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

        protected override IEnumerable<Control> GetChildren()
        {
            return _options;
        }
    }
}

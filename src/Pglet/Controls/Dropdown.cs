using System.Collections.Generic;

namespace Pglet.Controls
{
    public class Dropdown : Control
    {
        protected override string ControlName => "dropdown";

        IList<DropdownOption> _options = new List<DropdownOption>();
        public IList<DropdownOption> Options
        {
            get { return _options; }
            set { _options = value; }
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

        protected override IEnumerable<Control> GetChildren()
        {
            return _options;
        }
    }
}

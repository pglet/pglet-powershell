using System;
using System.Collections.Generic;
using System.Text;

namespace Pglet.Controls
{
    public class Dropdown : Control
    {
        public class Option : Control
        {
            protected override string ControlName => "option";

            public string Key
            {
                get { return GetAttr("key"); }
                set { SetAttr("key", value); }
            }

            public string Text
            {
                get { return GetAttr("text"); }
                set { SetAttr("text", value); }
            }
        }

        protected override string ControlName => "dropdown";

        IList<Option> _options = new List<Option>();
        public IList<Option> Options
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

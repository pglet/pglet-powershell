using System.Collections.Generic;

namespace Pglet.Controls
{
    public class ChoiceGroup : Control
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

            public string Icon
            {
                get { return GetAttr("icon"); }
                set { SetAttr("icon", value); }
            }

            public string IconColor
            {
                get { return GetAttr("iconColor"); }
                set { SetAttr("iconColor", value); }
            }
        }

        protected override string ControlName => "choicegroup";

        IList<Option> _options = new List<Option>();
        public IList<Option> Options
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

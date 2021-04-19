namespace Pglet.Controls
{
    public class SearchBox : Control
    {
        protected override string ControlName => "searchbox";

        public string Value
        {
            get { return GetAttr("value"); }
            set { SetAttr("value", value); }
        }

        public string Placeholder
        {
            get { return GetAttr("placeholder"); }
            set { SetAttr("placeholder", value); }
        }

        public bool Underlined
        {
            get { return GetBoolAttr("underlined"); }
            set { SetBoolAttr("underlined", value); }
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

        public EventHandler OnSearch
        {
            get { return GetEventHandler("search"); }
            set { SetEventHandler("search", value); }
        }

        public EventHandler OnClear
        {
            get { return GetEventHandler("clear"); }
            set { SetEventHandler("clear", value); }
        }

        public EventHandler OnEscape
        {
            get { return GetEventHandler("escape"); }
            set { SetEventHandler("escape", value); }
        }

        public EventHandler OnChange
        {
            get { return GetEventHandler("change"); }
            set
            {
                SetEventHandler("change", value);
                SetBoolAttr("onchange", value != null);
            }
        }
    }
}

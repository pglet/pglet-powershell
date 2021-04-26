namespace Pglet.Controls
{
    public class TextBox : Control
    {
        protected override string ControlName => "textbox";

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

        public string Description
        {
            get { return GetAttr("description"); }
            set { SetAttr("description", value); }
        }

        public string Prefix
        {
            get { return GetAttr("prefix"); }
            set { SetAttr("prefix", value); }
        }

        public string Suffix
        {
            get { return GetAttr("suffix"); }
            set { SetAttr("suffix", value); }
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

        public TextBoxAlign Align
        {
            get { return GetEnumAttr<TextBoxAlign>("align"); }
            set { SetEnumAttr("align", value); }
        }

        public bool Multiline
        {
            get { return GetBoolAttr("multiline"); }
            set { SetBoolAttr("multiline", value); }
        }

        public bool Password
        {
            get { return GetBoolAttr("password"); }
            set { SetBoolAttr("password", value); }
        }

        public bool Required
        {
            get { return GetBoolAttr("required"); }
            set { SetBoolAttr("required", value); }
        }

        public bool ReadOnly
        {
            get { return GetBoolAttr("readOnly"); }
            set { SetBoolAttr("readOnly", value); }
        }

        public bool AutoAdjustHeight
        {
            get { return GetBoolAttr("autoAdjustHeight"); }
            set { SetBoolAttr("autoAdjustHeight", value); }
        }

        public bool Underlined
        {
            get { return GetBoolAttr("underlined"); }
            set { SetBoolAttr("underlined", value); }
        }

        public bool Borderless
        {
            get { return GetBoolAttr("borderless"); }
            set { SetBoolAttr("borderless", value); }
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

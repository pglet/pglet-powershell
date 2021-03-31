using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Pglet.Controls
{
    public enum TextBoxAlign
    {
        [Description("left")]
        Left,

        [Description("right")]
        Right
    }

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

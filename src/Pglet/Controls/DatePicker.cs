using System;

namespace Pglet.Controls
{
    public class DatePicker : Control
    {
        protected override string ControlName => "datepicker";

        public string Label
        {
            get { return GetAttr("label"); }
            set { SetAttr("label", value); }
        }

        public DateTime? Value
        {
            get { return GetDateAttr("value"); }
            set { SetDateAttr("value", value); }
        }

        public string Placeholder
        {
            get { return GetAttr("placeholder"); }
            set { SetAttr("placeholder", value); }
        }

        public bool AllowTextInput
        {
            get { return GetBoolAttr("allowTextInput"); }
            set { SetBoolAttr("allowTextInput", value); }
        }

        public bool Required
        {
            get { return GetBoolAttr("required"); }
            set { SetBoolAttr("required", value); }
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
            set { SetEventHandler("change", value); }
        }
    }
}

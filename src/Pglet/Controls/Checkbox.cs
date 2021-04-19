namespace Pglet.Controls
{
    public class Checkbox : Control
    {
        protected override string ControlName => "checkbox";

        public string Label
        {
            get { return GetAttr("label"); }
            set { SetAttr("label", value); }
        }

        public bool Value
        {
            get { return GetBoolAttr("value"); }
            set { SetBoolAttr("value", value); }
        }

        public string ValueField
        {
            get { return GetAttr("value"); }
            set { SetAttr("value", $"{{{value}}}"); }
        }

        public BoxSide BoxSide
        {
            get { return GetEnumAttr<BoxSide>("boxSide"); }
            set { SetEnumAttr("boxSide", value); }
        }

        public EventHandler OnChange
        {
            get { return GetEventHandler("change"); }
            set { SetEventHandler("change", value); }
        }
    }
}

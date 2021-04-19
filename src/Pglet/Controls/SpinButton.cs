namespace Pglet.Controls
{
    public class SpinButton : Control
    {
        protected override string ControlName => "spinbutton";

        public string Label
        {
            get { return GetAttr("label"); }
            set { SetAttr("label", value); }
        }

        public float Value
        {
            get { return GetAttr<float>("value"); }
            set { SetAttr("value", value); }
        }

        public string ValueField
        {
            get { return GetAttr("value"); }
            set { SetAttr("value", $"{{{value}}}"); }
        }

        public string Icon
        {
            get { return GetAttr("icon"); }
            set { SetAttr("icon", value); }
        }

        public float Min
        {
            get { return GetAttr<float>("min"); }
            set { SetAttr("min", value); }
        }

        public float Max
        {
            get { return GetAttr<float>("max"); }
            set { SetAttr("max", value); }
        }

        public float Step
        {
            get { return GetAttr<float>("step"); }
            set { SetAttr("step", value); }
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

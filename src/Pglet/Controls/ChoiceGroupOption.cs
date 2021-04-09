namespace Pglet.Controls
{
    public partial class ChoiceGroup
    {
        public class ChoiceGroupOption : Control
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
    }
}

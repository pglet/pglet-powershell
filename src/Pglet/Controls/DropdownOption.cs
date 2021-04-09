namespace Pglet.Controls
{
    public partial class Dropdown
    {
        public class DropdownOption : Control
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
    }
}

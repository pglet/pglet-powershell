namespace Pglet.Controls
{
    public partial class Message
    {
        public class MessageButton : Control
        {
            protected override string ControlName => "button";

            public string Action
            {
                get { return GetAttr("action"); }
                set { SetAttr("action", value); }
            }

            public string Text
            {
                get { return GetAttr("text"); }
                set { SetAttr("text", value); }
            }
        }
    }
}

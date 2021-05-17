namespace Pglet.Controls
{
    public class Html : Control
    {
        protected override string ControlName => "html";

        public string Value
        {
            get { return GetAttr("value"); }
            set { SetAttr("value", value); }
        }
    }
}

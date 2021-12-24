namespace Pglet.Controls
{
    public class IFrame : Control
    {
        protected override string ControlName => "iframe";

        public string Src
        {
            get { return GetAttr("src"); }
            set { SetAttr("src", value); }
        }

        public string Title
        {
            get { return GetAttr("title"); }
            set { SetAttr("title", value); }
        }

        public string Border
        {
            get { return GetAttr("border"); }
            set { SetAttr("border", value); }
        }
    }
}

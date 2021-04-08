namespace Pglet.Controls
{
    public class Image : Control
    {
        protected override string ControlName => "image";

        public string Src
        {
            get { return GetAttr("src"); }
            set { SetAttr("src", value); }
        }

        public string Alt
        {
            get { return GetAttr("alt"); }
            set { SetAttr("alt", value); }
        }

        public string Title
        {
            get { return GetAttr("title"); }
            set { SetAttr("title", value); }
        }

        public bool MaximizeFrame
        {
            get { return GetBoolAttr("maximizeFrame"); }
            set { SetBoolAttr("maximizeFrame", value); }
        }
    }
}

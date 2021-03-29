using System;
using System.Collections.Generic;
using System.Text;

namespace Pglet.Controls
{
    public class Link : Control
    {
        protected override string ControlName => "link";

        IList<Control> _controls = new List<Control>();
        public IList<Control> Controls
        {
            get { return _controls; }
            set { _controls = value; }
        }

        public string Value
        {
            get { return GetAttr("value"); }
            set { SetAttr("value", value); }
        }

        public string Url
        {
            get { return GetAttr("url"); }
            set { SetAttr("url", value); }
        }

        public string Title
        {
            get { return GetAttr("title"); }
            set { SetAttr("title", value); }
        }

        public string Size
        {
            get { return GetAttr("size"); }
            set { SetAttr("size", value); }
        }

        public bool Bold
        {
            get { return GetBoolAttr("bold"); }
            set { SetBoolAttr("bold", value); }
        }

        public bool Italic
        {
            get { return GetBoolAttr("italic"); }
            set { SetBoolAttr("italic", value); }
        }

        public bool Pre
        {
            get { return GetBoolAttr("pre"); }
            set { SetBoolAttr("pre", value); }
        }

        public bool NewWindow
        {
            get { return GetBoolAttr("newWindow"); }
            set { SetBoolAttr("newWindow", value); }
        }

        public TextAlign Align
        {
            get { return GetEnumAttr<TextAlign>("align"); }
            set { SetEnumAttr("align", value); }
        }

        public EventHandler OnClick
        {
            get { return GetEventHandler("click"); }
            set { SetEventHandler("click", value); }
        }

        protected override IEnumerable<Control> GetChildren()
        {
            return _controls;
        }
    }
}

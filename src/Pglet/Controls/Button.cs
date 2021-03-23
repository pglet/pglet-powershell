using System;
using System.Collections.Generic;
using System.Text;

namespace Pglet.Controls
{
    public class Button : Control
    {
        protected override string ControlName => "button";

        public bool Primary
        {
            get { return GetBoolAttr("primary"); }
            set { SetBoolAttr("primary", value); }
        }

        public bool Compound
        {
            get { return GetBoolAttr("compound"); }
            set { SetBoolAttr("compound", value); }
        }

        public bool Action
        {
            get { return GetBoolAttr("action"); }
            set { SetBoolAttr("action", value); }
        }

        public bool Toolbar
        {
            get { return GetBoolAttr("toolbar"); }
            set { SetBoolAttr("toolbar", value); }
        }

        public bool Split
        {
            get { return GetBoolAttr("split"); }
            set { SetBoolAttr("split", value); }
        }

        public string Text
        {
            get { return GetAttr("text"); }
            set { SetAttr("text", value); }
        }

        public string SecondaryText
        {
            get { return GetAttr("secondaryText"); }
            set { SetAttr("secondaryText", value); }
        }

        public string Url
        {
            get { return GetAttr("url"); }
            set { SetAttr("url", value); }
        }

        public bool NewWindow
        {
            get { return GetBoolAttr("newWindow"); }
            set { SetBoolAttr("newWindow", value); }
        }

        public string Title
        {
            get { return GetAttr("title"); }
            set { SetAttr("title", value); }
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

        public string Data
        {
            get { return GetAttr("data"); }
            set { SetAttr("data", value); }
        }

        public EventHandler OnClick
        {
            get { return GetEventHandler("click"); }
            set { SetEventHandler("click", value); }
        }
    }
}

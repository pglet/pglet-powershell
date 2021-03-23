using System;
using System.Collections.Generic;
using System.Text;

namespace Pglet.Controls
{
    public class Textbox : Control
    {
        protected override string ControlName => "textbox";

        public string Label
        {
            get { return GetAttr("label"); }
            set { SetAttr("label", value); }
        }

        public string Value
        {
            get { return GetAttr("value"); }
            set { SetAttr("value", value); }
        }

        public bool Multiline
        {
            get { return GetBoolAttr("multiline"); }
            set { SetBoolAttr("multiline", value); }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Pglet.Controls
{
    public class Text : Control
    {
        protected override string ControlName => "text";

        public string Value
        {
            get { return GetAttr("value"); }
            set { SetAttr("value", value); }
        }

        public string Color
        {
            get { return GetAttr("color"); }
            set { SetAttr("color", value); }
        }

        public string Size
        {
            get { return GetAttr("size"); }
            set { SetAttr("size", value); }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Pglet.Controls
{
    public class Icon : Control
    {
        protected override string ControlName => "icon";
        public string Name
        {
            get { return GetAttr("name"); }
            set { SetAttr("name", value); }
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

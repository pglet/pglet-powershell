using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Pglet.Controls
{
    public class Progress : Control
    {
        protected override string ControlName => "progress";

        public int Value
        {
            get { return GetIntAttr("value"); }
            set { SetIntAttr("value", value); }
        }

        public string Label
        {
            get { return GetAttr("label"); }
            set { SetAttr("label", value); }
        }

        public string Description
        {
            get { return GetAttr("description"); }
            set { SetAttr("description", value); }
        }
    }
}

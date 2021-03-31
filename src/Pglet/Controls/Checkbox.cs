﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Pglet.Controls
{
    public enum BoxSide
    {
        [Description("start")]
        Start,

        [Description("end")]
        End
    }

    public class Checkbox : Control
    {
        protected override string ControlName => "checkbox";

        public string Label
        {
            get { return GetAttr("label"); }
            set { SetAttr("label", value); }
        }

        public bool Value
        {
            get { return GetBoolAttr("value"); }
            set { SetBoolAttr("value", value); }
        }

        public BoxSide BoxSide
        {
            get { return GetEnumAttr<BoxSide>("boxSide"); }
            set { SetEnumAttr("boxSide", value); }
        }

        public EventHandler OnChange
        {
            get { return GetEventHandler("change"); }
            set { SetEventHandler("change", value); }
        }
    }
}

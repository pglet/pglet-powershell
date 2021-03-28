﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Pglet.Controls
{
    public class Toggle : Control
    {
        protected override string ControlName => "toggle";

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

        public bool Inline
        {
            get { return GetBoolAttr("inline"); }
            set { SetBoolAttr("inline", value); }
        }

        public string OnText
        {
            get { return GetAttr("onText"); }
            set { SetAttr("onText", value); }
        }

        public string OffText
        {
            get { return GetAttr("offText"); }
            set { SetAttr("offText", value); }
        }

        public EventHandler OnChange
        {
            get { return GetEventHandler("change"); }
            set { SetEventHandler("change", value); }
        }
    }
}

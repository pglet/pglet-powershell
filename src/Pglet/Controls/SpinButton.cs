using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Pglet.Controls
{
    public class SpinButton : Control
    {
        protected override string ControlName => "spinbutton";

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

        public string Icon
        {
            get { return GetAttr("icon"); }
            set { SetAttr("icon", value); }
        }

        public float Min
        {
            get { return GetAttr<float>("min"); }
            set { SetAttr("min", value); }
        }

        public float Max
        {
            get { return GetAttr<float>("max"); }
            set { SetAttr("max", value); }
        }

        public float Step
        {
            get { return GetAttr<float>("step"); }
            set { SetAttr("step", value); }
        }

        public EventHandler OnChange
        {
            get { return GetEventHandler("change"); }
            set
            {
                SetEventHandler("change", value);
                SetBoolAttr("onchange", value != null);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Pglet.Controls
{
    public class Slider : Control
    {
        protected override string ControlName => "slider";
        
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

        public string ValueFormat
        {
            get { return GetAttr("valueFormat"); }
            set { SetAttr("valueFormat", value); }
        }

        public bool ShowValue
        {
            get { return GetBoolAttr("showValue"); }
            set { SetBoolAttr("showValue", value); }
        }

        public bool Vertical
        {
            get { return GetBoolAttr("vertical"); }
            set { SetBoolAttr("vertical", value); }
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

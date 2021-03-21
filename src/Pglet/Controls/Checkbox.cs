using System;
using System.Collections.Generic;
using System.Text;

namespace Pglet.Controls
{
    public class Checkbox : Control
    {
        protected override string ControlName => "checkbox";

        public Checkbox(
            string label = null, bool? value = null, EventHandler onChange = null,
            string id = null, string width = null, string height = null, string padding = null, string margin = null,
            bool? visible = null, bool? disabled = null) : base(id: id, width: width, height: height, padding: padding, margin: margin, visible: visible, disabled: disabled)
        {
            Label = label;

            if (value.HasValue)
            {
                Value = value.Value;
            }

            OnChange = onChange;
        }

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

        public EventHandler OnChange
        {
            get { return GetEventHandler("change"); }
            set { SetEventHandler("change", value); }
        }
    }
}

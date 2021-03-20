using System;
using System.Collections.Generic;
using System.Text;

namespace Pglet.Controls
{
    public class Textbox : Control
    {
        protected override string ControlName => "textbox";

        public Textbox(
            string label = null, string value = null, bool? multiline = null,
            string id = null, string width = null, string height = null, string padding = null, string margin = null,
            bool? visible = null, bool? disabled = null) : base(id: id, width: width, height: height, padding: padding, margin: margin, visible: visible, disabled: disabled)
        {
            Label = label;
            Value = value;

            if (multiline.HasValue)
            {
                Multiline = multiline.Value;
            }
        }

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

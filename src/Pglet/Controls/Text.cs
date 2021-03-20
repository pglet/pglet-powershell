using System;
using System.Collections.Generic;
using System.Text;

namespace Pglet.Controls
{
    public class Text : Control
    {
        protected override string ControlName => "text";

        public Text(
            string value = null, string color = null, string size = null,
            string id = null, string width = null, string height = null, string padding = null, string margin = null,
            bool? visible = null, bool? disabled = null) : base(id: id, width: width, height: height, padding: padding, margin: margin, visible: visible, disabled: disabled)
        {
            Value = value;
            Color = color;
            Size = size;
        }

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

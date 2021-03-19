using System;
using System.Collections.Generic;
using System.Text;

namespace Pglet.Controls
{
    public class Icon : Control
    {
        protected override string ControlName => "icon";

        public Icon(
            string name = null, string color = null, string size = null,
            string id = null, string width = null, string height = null, string padding = null, string margin = null,
            bool? visible = null, bool? disabled = null) : base(id: id, width: width, height: height, padding: padding, margin: margin, visible: visible, disabled: disabled)
        {
            Name = name;
            Color = color;
            Size = size;
        }

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

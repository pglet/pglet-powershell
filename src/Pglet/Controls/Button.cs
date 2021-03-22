using System;
using System.Collections.Generic;
using System.Text;

namespace Pglet.Controls
{
    public class Button : Control
    {
        protected override string ControlName => "button";

        public Button(
            string text = null,
            EventHandler onClick = null,

            // standard props
            string id = null,
            string width = null,
            string height = null,
            string padding = null,
            string margin = null,
            bool? visible = null,
            bool? disabled = null) : base(
                id: id,
                width: width,
                height: height,
                padding: padding,
                margin: margin,
                visible: visible,
                disabled: disabled)
        {
            Text = text;
            OnClick = onClick;
        }

        public string Text
        {
            get { return GetAttr("text"); }
            set { SetAttr("text", value); }
        }

        public EventHandler OnClick
        {
            get { return GetEventHandler("click"); }
            set { SetEventHandler("click", value); }
        }
    }
}

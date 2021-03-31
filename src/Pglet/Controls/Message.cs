using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Pglet.Controls
{
    public enum MessageType
    {
        [Description("info")]
        Info,

        [Description("error")]
        Error,

        [Description("blocked")]
        Blocked,

        [Description("severeWarning")]
        SevereWarning,

        [Description("success")]
        Success,

        [Description("warning")]
        Warning
    }

    public class Message : Control
    {
        public class Button : Control
        {
            protected override string ControlName => "button";

            public string Action
            {
                get { return GetAttr("action"); }
                set { SetAttr("action", value); }
            }

            public string Text
            {
                get { return GetAttr("text"); }
                set { SetAttr("text", value); }
            }
        }

        protected override string ControlName => "message";

        IList<Button> _buttons = new List<Button>();
        public IList<Button> Buttons
        {
            get { return _buttons; }
            set { _buttons = value; }
        }

        public MessageType Type
        {
            get { return GetEnumAttr<MessageType>("type"); }
            set { SetEnumAttr("type", value); }
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

        public bool Truncated
        {
            get { return GetBoolAttr("truncated"); }
            set { SetBoolAttr("truncated", value); }
        }

        public bool Dismiss
        {
            get { return GetBoolAttr("dismiss"); }
            set { SetBoolAttr("dismiss", value); }
        }

        public EventHandler OnDismiss
        {
            get { return GetEventHandler("dismiss"); }
            set { SetEventHandler("dismiss", value); }
        }

        protected override IEnumerable<Control> GetChildren()
        {
            return _buttons;
        }
    }
}

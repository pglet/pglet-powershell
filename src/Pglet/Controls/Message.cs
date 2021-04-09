using System.Collections.Generic;

namespace Pglet.Controls
{
    public partial class Message : Control
    {
        protected override string ControlName => "message";

        IList<MessageButton> _buttons = new List<MessageButton>();
        public IList<MessageButton> Buttons
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

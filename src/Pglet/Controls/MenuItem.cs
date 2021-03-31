using System.Collections.Generic;

namespace Pglet.Controls
{
    public class MenuItem : Control
    {
        IList<MenuItem> _subMenuItems = new List<MenuItem>();

        protected override string ControlName => "item";

        public IList<MenuItem> SubMenuItems
        {
            get { return _subMenuItems; }
            set { _subMenuItems = value; }
        }

        public string Text
        {
            get { return GetAttr("text"); }
            set { SetAttr("text", value); }
        }

        public string SecondaryText
        {
            get { return GetAttr("secondaryText"); }
            set { SetAttr("secondaryText", value); }
        }

        public string Url
        {
            get { return GetAttr("url"); }
            set { SetAttr("url", value); }
        }

        public bool NewWindow
        {
            get { return GetBoolAttr("newWindow"); }
            set { SetBoolAttr("newWindow", value); }
        }

        public string Icon
        {
            get { return GetAttr("icon"); }
            set { SetAttr("icon", value); }
        }

        public string IconColor
        {
            get { return GetAttr("iconColor"); }
            set { SetAttr("iconColor", value); }
        }

        public bool IconOnly
        {
            get { return GetBoolAttr("iconOnly"); }
            set { SetBoolAttr("iconOnly", value); }
        }

        public bool Split
        {
            get { return GetBoolAttr("split"); }
            set { SetBoolAttr("split", value); }
        }

        public bool Divider
        {
            get { return GetBoolAttr("divider"); }
            set { SetBoolAttr("divider", value); }
        }

        public EventHandler OnClick
        {
            get { return GetEventHandler("click"); }
            set { SetEventHandler("click", value); }
        }

        protected override IEnumerable<Control> GetChildren()
        {
            return _subMenuItems;
        }
    }
}

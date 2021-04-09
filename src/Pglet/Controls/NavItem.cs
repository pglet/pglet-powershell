using System.Collections.Generic;

namespace Pglet.Controls
{
    public partial class Nav
    {
        public class NavItem : Control
        {
            IList<NavItem> _subItems = new List<NavItem>();

            protected override string ControlName => "item";

            public IList<NavItem> SubItems
            {
                get { return _subItems; }
                set { _subItems = value; }
            }

            public string Key
            {
                get { return GetAttr("key"); }
                set { SetAttr("key", value); }
            }

            public string Text
            {
                get { return GetAttr("text"); }
                set { SetAttr("text", value); }
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

            public bool Expanded
            {
                get { return GetBoolAttr("expanded"); }
                set { SetBoolAttr("expanded", value); }
            }

            protected override IEnumerable<Control> GetChildren()
            {
                return _subItems;
            }
        }
    }
}

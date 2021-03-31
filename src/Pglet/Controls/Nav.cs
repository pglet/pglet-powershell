using System;
using System.Collections.Generic;
using System.Text;

namespace Pglet.Controls
{
    public partial class Nav : Control
    {
        public class Item : Control
        {
            IList<Item> _subItems = new List<Item>();

            protected override string ControlName => "item";

            public IList<Item> SubItems
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
        protected override string ControlName => "nav";

        IList<Item> _items = new List<Item>();
        public IList<Item> Items
        {
            get { return _items; }
            set { _items = value; }
        }

        public string Value
        {
            get { return GetAttr("value"); }
            set { SetAttr("value", value); }
        }

        public EventHandler OnChange
        {
            get { return GetEventHandler("change"); }
            set { SetEventHandler("change", value); }
        }

        public EventHandler OnExpand
        {
            get { return GetEventHandler("expand"); }
            set { SetEventHandler("expand", value); }
        }

        public EventHandler OnCollapse
        {
            get { return GetEventHandler("collapse"); }
            set { SetEventHandler("collapse", value); }
        }

        protected override IEnumerable<Control> GetChildren()
        {
            return _items;
        }
    }
}

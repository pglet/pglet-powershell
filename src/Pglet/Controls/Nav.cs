using System.Collections.Generic;

namespace Pglet.Controls
{
    public partial class Nav : Control
    {
        protected override string ControlName => "nav";

        IList<NavItem> _items = new List<NavItem>();
        public IList<NavItem> Items
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

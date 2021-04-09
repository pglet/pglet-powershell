using System.Collections.Generic;

namespace Pglet.Controls
{
    public class Toolbar : Control
    {
        protected override string ControlName => "toolbar";

        IList<MenuItem> _items = new List<MenuItem>();
        readonly ToolbarOverflow _overflow = new();
        readonly ToolbarFar _far = new();

        public IList<MenuItem> Items
        {
            get { return _items; }
            set { _items = value; }
        }

        public IList<MenuItem> OverflowItems
        {
            get { return _overflow.MenuItems; }
            set { _overflow.MenuItems = value; }
        }

        public IList<MenuItem> FarItems
        {
            get { return _far.MenuItems; }
            set { _far.MenuItems = value; }
        }

        public bool Inverted
        {
            get { return GetBoolAttr("inverted"); }
            set { SetBoolAttr("inverted", value); }
        }

        protected override IEnumerable<Control> GetChildren()
        {
            var controls = new List<Control>();
            controls.AddRange(_items);
            controls.Add(_overflow);
            controls.Add(_far);
            return controls;
        }
    }
}

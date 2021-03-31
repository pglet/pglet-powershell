using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Pglet.Controls
{
    public class Toolbar : Control
    {
        public class Overflow : Control
        {
            IList<MenuItem> _menuItems = new List<MenuItem>();

            protected override string ControlName => "overflow";

            public IList<MenuItem> MenuItems
            {
                get { return _menuItems; }
                set { _menuItems = value; }
            }

            protected override IEnumerable<Control> GetChildren()
            {
                return _menuItems;
            }
        }

        public class Far : Control
        {
            IList<MenuItem> _menuItems = new List<MenuItem>();

            protected override string ControlName => "far";

            public IList<MenuItem> MenuItems
            {
                get { return _menuItems; }
                set { _menuItems = value; }
            }

            protected override IEnumerable<Control> GetChildren()
            {
                return _menuItems;
            }
        }

        protected override string ControlName => "toolbar";

        IList<MenuItem> _items = new List<MenuItem>();
        readonly Overflow _overflow = new();
        readonly Far _far = new();

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

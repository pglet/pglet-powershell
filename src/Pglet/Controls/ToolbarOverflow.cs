using System.Collections.Generic;

namespace Pglet.Controls
{
    public partial class Toolbar
    {
        public class ToolbarOverflow : Control
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
    }
}

using System.Collections.Generic;

namespace Pglet.Controls
{
    public class Grid : Control
    {
        protected override string ControlName => "grid";

        readonly GridColumns _columns = new();
        readonly GridItems _items = new();

        public IList<GridColumn> Columns
        {
            get { return _columns.Items; }
            set { _columns.Items = value; }
        }

        public IList<object> Items
        {
            get { return _items.Items; }
            set { _items.Items = value; }
        }

        public GridSelectionMode SelectionMode
        {
            get { return GetEnumAttr<GridSelectionMode>("selection"); }
            set { SetEnumAttr("selection", value); }
        }

        public bool Compact
        {
            get { return GetBoolAttr("compact"); }
            set { SetBoolAttr("compact", value); }
        }

        public bool HeaderVisible
        {
            get { return GetBoolAttr("headerVisible"); }
            set { SetBoolAttr("headerVisible", value); }
        }

        public int ShimmerLines
        {
            get { return GetIntAttr("shimmerLines"); }
            set { SetIntAttr("shimmerLines", value); }
        }

        public EventHandler OnSelect
        {
            get { return GetEventHandler("select"); }
            set { SetEventHandler("select", value); }
        }

        public EventHandler OnItemInvoke
        {
            get { return GetEventHandler("itemInvoke"); }
            set { SetEventHandler("itemInvoke", value); }
        }

        protected override IEnumerable<Control> GetChildren()
        {
            return new Control[] { _columns, _items };
        }
    }
}

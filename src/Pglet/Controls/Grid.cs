using System.Collections.Generic;
using System.Linq;

namespace Pglet.Controls
{
    public class Grid : Control
    {
        protected override string ControlName => "grid";

        readonly GridColumns _columns = new();
        readonly GridItems _items = new();
        IList<object> _selectedItems = new List<object>();

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

        public IList<object> SelectedItems
        {
            get { return _selectedItems; }
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

        public bool PreserveSelection
        {
            get { return GetBoolAttr("preserveSelection"); }
            set { SetBoolAttr("preserveSelection", value); }
        }

        public int ShimmerLines
        {
            get { return GetIntAttr("shimmerLines"); }
            set { SetIntAttr("shimmerLines", value); }
        }

        EventHandler _onSelectHandler;
        public EventHandler OnSelect
        {
            get { return _onSelectHandler; }
            set
            {
                _onSelectHandler = value;
                if (_onSelectHandler != null)
                {
                    SetEventHandler("select", (e) =>
                    {
                        OnSelectInternal(e);
                    });
                }
                else
                {
                    SetEventHandler("select", null);
                }
            }
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

        protected void OnSelectInternal(ControlEvent e)
        {
            _selectedItems = e.Data.Split(' ').Where(id => id != "").Select(id => (this.Page.GetControl(id) as GridItem).Obj).ToList();
            _onSelectHandler?.Invoke(e);
        }
    }
}

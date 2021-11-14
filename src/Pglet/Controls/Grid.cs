using Newtonsoft.Json;
using System;
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
            get { return _columns.Columns; }
            set { _columns.Columns = value; }
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

        public int? ShimmerLines
        {
            get { return GetNullableIntAttr("shimmerLines"); }
            set { SetNullableIntAttr("shimmerLines", value); }
        }

        public string KeyFieldName { get; set; }

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
            if (_columns.Columns.Count == 0 && _items.Items.Count > 0)
            {
                // auto-generate columns
                _columns.Columns = GridHelper.GenerateColumns(_items.Items[0]);
            }

            var fetchPropNames = new List<string>();
            foreach (var column in _columns.Columns)
            {
                if (column.TemplateControls.Count == 0)
                {
                    fetchPropNames.Add(column.FieldName);
                }
                else
                {
                    fetchPropNames = null;
                    break;
                }
            }

            foreach (var column in _columns.Columns)
            {
                column.FieldName = GridHelper.EncodeReservedProperty(column.FieldName);
            }

            if (fetchPropNames != null && !String.IsNullOrEmpty(KeyFieldName) && !fetchPropNames.Contains(KeyFieldName, StringComparer.OrdinalIgnoreCase))
            {
                fetchPropNames.Add(KeyFieldName);
            }

            _items.FetchPropNames = fetchPropNames;
            _items.KeyFieldName = KeyFieldName;

            return new Control[] { _columns, _items };
        }

        protected void OnSelectInternal(ControlEvent e)
        {
            _selectedItems = e.Data.Split(' ').Where(id => id != "").Select(id => (this.Page.GetControl(id) as GridItem).Obj).ToList();
            _onSelectHandler?.Invoke(e);
        }
    }
}

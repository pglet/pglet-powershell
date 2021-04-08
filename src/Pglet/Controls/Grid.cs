using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Pglet.Controls
{
    public enum GridSelectionMode
    {
        [Description("none")]
        None,

        [Description("single ")]
        Single,

        [Description("multiple")]
        Multiple
    }

    public class Grid : Control
    {
        public class Item : Control
        {
            protected override string ControlName => "item";

            object _obj;

            public object Obj
            {
                get { return _obj; }
            }

            public Item(object obj)
            {
                if (obj == null)
                {
                    throw new ArgumentNullException("obj");
                }

                _obj = obj;
            }

            internal override void SetAttr(string name, string value, bool dirty = true)
            {
                base.SetAttr(name, value, false);

                var prop = _obj.GetType().GetProperty(name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (prop != null)
                {
                    prop.SetValue(_obj, Convert.ChangeType(value, prop.PropertyType), null);
                }
            }

            internal void FetchAttrs()
            {
                foreach(var prop in _obj.GetType().GetProperties())
                {
                    var val = prop.GetValue(_obj);
                    if (val != null)
                    {
                        var sval = val.ToString();

                        if (prop.PropertyType == typeof(bool))
                        {
                            sval = sval.ToLowerInvariant();
                        }

                        var origSval = this.GetAttr(prop.Name);

                        if (sval != origSval)
                        {
                            base.SetAttr(prop.Name, sval, dirty: true);
                        }
                    }
                }
            }
        }

        public class Column: Control
        {
            protected override string ControlName => "column";

            public string Name
            {
                get { return GetAttr("name"); }
                set { SetAttr("name", value); }
            }

            public string Icon
            {
                get { return GetAttr("icon"); }
                set { SetAttr("icon", value); }
            }

            public bool IconOnly
            {
                get { return GetBoolAttr("iconOnly"); }
                set { SetBoolAttr("iconOnly", value); }
            }

            public string FieldName
            {
                get { return GetAttr("fieldName"); }
                set { SetAttr("fieldName", value); }
            }

            public string Sortable
            {
                get { return GetAttr("sortable"); }
                set { SetAttr("sortable", value); }
            }

            public string SortField
            {
                get { return GetAttr("sortField"); }
                set { SetAttr("sortField", value); }
            }

            public string Sorted
            {
                get { return GetAttr("sorted"); }
                set { SetAttr("sorted", value); }
            }

            public bool Resizable
            {
                get { return GetBoolAttr("resizable"); }
                set { SetBoolAttr("resizable", value); }
            }

            public int MinWidth
            {
                get { return GetIntAttr("minWidth"); }
                set { SetIntAttr("minWidth", value); }
            }

            public int MaxWidth
            {
                get { return GetIntAttr("maxWidth"); }
                set { SetIntAttr("maxWidth", value); }
            }

            public EventHandler OnClick
            {
                get { return GetEventHandler("click"); }
                set { SetEventHandler("click", value); }
            }

            IList<Control> _templateControls = new List<Control>();
            public IList<Control> TemplateControls
            {
                get { return _templateControls; }
                set { _templateControls = value; }
            }

            protected override IEnumerable<Control> GetChildren()
            {
                return _templateControls;
            }
        }

        public class GridColumns : Control
        {
            IList<Column> _columns = new List<Column>();

            protected override string ControlName => "columns";

            public IList<Column> Items
            {
                get { return _columns; }
                set { _columns = value; }
            }

            protected override IEnumerable<Control> GetChildren()
            {
                return _columns;
            }
        }

        public class GridItems : Control
        {
            protected override string ControlName => "items";

            Dictionary<object, Item> _map = new Dictionary<object, Item>();

            IList<object> _items = new List<object>();
            public IList<object> Items
            {
                get { return _items; }
                set { _items = value; }
            }

            protected override IEnumerable<Control> GetChildren()
            {
                var items = new List<Item>();
                foreach(var obj in _items)
                {
                    Item item;
                    if (_map.ContainsKey(obj))
                    {
                        item = _map[obj];
                    }
                    else
                    {
                        item = new Item(obj);
                        _map[obj] = item;
                    }
                    item.FetchAttrs();
                    items.Add(item);
                }

                // delete items that are not in list
                foreach(var deletedItem in _map.Values.Except(items).ToList())
                {
                    _map.Remove(deletedItem.Obj);
                }

                return items;
            }
        }

        protected override string ControlName => "grid";

        readonly GridColumns _columns = new();
        readonly GridItems _items = new();

        public IList<Column> Columns
        {
            get { return _columns.Items; }
            set { _columns.Items = value; }
        }

        public IList<object> Items
        {
            get { return _items.Items; }
            set { _items.Items = value; }
        }

        public GridSelectionMode Selection
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

        protected override IEnumerable<Control> GetChildren()
        {
            return new Control[] { _columns, _items };
        }
    }
}

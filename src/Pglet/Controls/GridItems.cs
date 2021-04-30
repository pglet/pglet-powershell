using System.Collections.Generic;
using System.Linq;

namespace Pglet.Controls
{
    public class GridItems : Control
    {
        protected override string ControlName => "items";

        Dictionary<object, GridItem> _map = new Dictionary<object, GridItem>();

        internal IEnumerable<string> FetchPropNames { get; set; }
        internal string KeyFieldName { get; set; }

        IList<object> _items = new List<object>();
        public IList<object> Items
        {
            get { return _items; }
            set { _items = value; }
        }

        protected override IEnumerable<Control> GetChildren()
        {
            var items = new List<GridItem>();
            foreach (var obj in _items)
            {
                GridItem item;
                if (_map.ContainsKey(obj))
                {
                    item = _map[obj];
                }
                else
                {
                    item = new GridItem(obj);
                    _map[obj] = item;
                }
                item.FetchAttrs(FetchPropNames, KeyFieldName);
                items.Add(item);
            }

            // delete items that are not in list
            foreach (var deletedItem in _map.Values.Except(items).ToList())
            {
                _map.Remove(deletedItem.Obj);
            }

            return items;
        }
    }
}

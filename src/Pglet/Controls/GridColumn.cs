using System.Collections.Generic;

namespace Pglet.Controls
{
    public class GridColumn : Control
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
}

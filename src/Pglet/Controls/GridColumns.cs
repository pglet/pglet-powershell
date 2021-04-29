using System.Collections.Generic;

namespace Pglet.Controls
{
    public class GridColumns : Control
    {
        IList<GridColumn> _columns = new List<GridColumn>();

        protected override string ControlName => "columns";

        public IList<GridColumn> Columns
        {
            get { return _columns; }
            set { _columns = value; }
        }

        protected override IEnumerable<Control> GetChildren()
        {
            return _columns;
        }
    }
}

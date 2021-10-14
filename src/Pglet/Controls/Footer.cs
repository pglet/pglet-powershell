using System.Collections.Generic;

namespace Pglet.Controls
{
    public class Footer : Control
    {
        protected override string ControlName => "footer";

        IList<Control> _controls = new List<Control>();

        public IList<Control> Controls
        {
            get
            {
                return _controls;
            }
            set
            {
                _controls = value;
            }
        }

        protected override IEnumerable<Control> GetChildren()
        {
            return _controls;
        }
    }
}

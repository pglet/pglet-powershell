using System;
using System.Collections.Generic;
using System.Text;

namespace Pglet.Controls
{
    public class Stack : Control
    {
        protected override string ControlName => "stack";

        IList<Control> _controls = new List<Control>();

        public IList<Control> Controls
        {
            get { return _controls; }
            set { _controls = value; }
        }

        protected override IEnumerable<Control> GetChildren()
        {
            return _controls;
        }
    }
}

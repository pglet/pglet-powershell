using System;
using System.Collections.Generic;
using System.Text;

namespace Pglet.Controls
{
    public class Stack : Control
    {
        IList<Control> _controls = new List<Control>();

        public IList<Control> Controls
        {
            get { return _controls; }
            set { _controls = value; }
        }

        protected override string ControlName => "stack";

        public Stack(
                    IEnumerable<Control> controls = null,
                    string id = null, string width = null, string height = null, string padding = null, string margin = null,
                    bool? visible = null, bool? disabled = null) : base(id: id, width: width, height: height, padding: padding, margin: margin, visible: visible, disabled: disabled)
        {
            if (controls != null)
            {
                foreach(var control in controls)
                {
                    _controls.Add(control);
                }
            }
        }

        protected override IEnumerable<Control> GetChildren()
        {
            return _controls;
        }
    }
}

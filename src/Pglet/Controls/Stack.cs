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

        public bool Horizontal
        {
            get { return GetBoolAttr("horizontal"); }
            set { SetBoolAttr("horizontal", value); }
        }

        public bool VerticalFill
        {
            get { return GetBoolAttr("verticalFill"); }
            set { SetBoolAttr("verticalFill", value); }
        }

        public Align HorizontalAlign
        {
            get { return GetEnumAttr<Align>("horizontalAlign"); }
            set { SetEnumAttr("horizontalAlign", value); }
        }

        public Align VerticalAlign
        {
            get { return GetEnumAttr<Align>("verticalAlign"); }
            set { SetEnumAttr("verticalAlign", value); }
        }

        protected override IEnumerable<Control> GetChildren()
        {
            return _controls;
        }
    }
}

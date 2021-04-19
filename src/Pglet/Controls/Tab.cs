using System.Collections.Generic;

namespace Pglet.Controls
{
    public class Tab : Control
    {
        protected override string ControlName => "tab";

        IList<Control> _controls = new List<Control>();

        public IList<Control> Controls
        {
            get { return _controls; }
            set { _controls = value; }
        }

        public string Key
        {
            get { return GetAttr("key"); }
            set { SetAttr("key", value); }
        }

        public string Text
        {
            get { return GetAttr("text"); }
            set { SetAttr("text", value); }
        }

        public string Icon
        {
            get { return GetAttr("icon"); }
            set { SetAttr("icon", value); }
        }

        public string Count
        {
            get { return GetAttr("count"); }
            set { SetAttr("count", value); }
        }

        protected override IEnumerable<Control> GetChildren()
        {
            return _controls;
        }
    }
}

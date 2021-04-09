using System.Collections.Generic;

namespace Pglet.Controls
{
    public class Panel : Control
    {
        protected override string ControlName => "panel";

        Footer _footer = new Footer();
        public Footer Footer
        {
            get { return _footer; }
            set { _footer = value; }
        }

        IList<Control> _controls = new List<Control>();
        public IList<Control> Controls
        {
            get { return _controls; }
            set { _controls = value; }
        }

        public PanelType Type
        {
            get { return GetEnumAttr<PanelType>("type"); }
            set { SetEnumAttr("type", value); }
        }

        public bool Open
        {
            get { return GetBoolAttr("open"); }
            set { SetBoolAttr("open", value); }
        }

        public string Title
        {
            get { return GetAttr("title"); }
            set { SetAttr("title", value); }
        }

        public bool AutoDismiss
        {
            get { return GetBoolAttr("autoDismiss"); }
            set { SetBoolAttr("autoDismiss", value); }
        }

        public bool LightDismiss
        {
            get { return GetBoolAttr("lightDismiss"); }
            set { SetBoolAttr("lightDismiss", value); }
        }

        public bool Blocking
        {
            get { return GetBoolAttr("blocking"); }
            set { SetBoolAttr("blocking", value); }
        }

        public EventHandler OnDismiss
        {
            get { return GetEventHandler("dismiss"); }
            set { SetEventHandler("dismiss", value); }
        }

        protected override IEnumerable<Control> GetChildren()
        {
            var controls = new List<Control>();
            controls.AddRange(_controls);
            controls.Add(_footer);
            return controls;
        }
    }
}

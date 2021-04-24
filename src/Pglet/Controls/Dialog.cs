using System.Collections.Generic;

namespace Pglet.Controls
{
    public class Dialog : Control
    {
        protected override string ControlName => "dialog";

        Footer _footer = new Footer();
        public IList<Control> FooterControls
        {
            get { return _footer.Controls; }
            set { _footer.Controls = value; }
        }

        IList<Control> _controls = new List<Control>();
        public IList<Control> Controls
        {
            get { return _controls; }
            set { _controls = value; }
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

        public string SubText
        {
            get { return GetAttr("subText"); }
            set { SetAttr("subText", value); }
        }

        public DialogType Type
        {
            get { return GetEnumAttr<DialogType>("type"); }
            set { SetEnumAttr("type", value); }
        }

        public bool AutoDismiss
        {
            get { return GetBoolAttr("autoDismiss"); }
            set { SetBoolAttr("autoDismiss", value); }
        }

        public string MaxWidth
        {
            get { return GetAttr("maxWidth"); }
            set { SetAttr("maxWidth", value); }
        }

        public bool FixedTop
        {
            get { return GetBoolAttr("fixedTop"); }
            set { SetBoolAttr("fixedTop", value); }
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

using System.Collections.Generic;
using System.ComponentModel;

namespace Pglet.Controls
{
    public enum CalloutPosition
    {
        [Description("topLeft")]
        TopLeft,

        [Description("topCenter")]
        TopCenter,

        [Description("topRight")]
        TopRight,

        [Description("topAuto")]
        TopAuto,

        [Description("bottomLeft")]
        BottomLeft,

        [Description("bottomCenter")]
        BottomCenter,

        [Description("bottomRight")]
        BottomRight,

        [Description("bottomAuto")]
        BottomAuto,

        [Description("leftTop")]
        LeftTop,

        [Description("leftCenter")]
        LeftCenter,

        [Description("leftBottom")]
        LeftBottom,

        [Description("rightTop")]
        RightTop,

        [Description("rightCenter")]
        RightCenter,

        [Description("rightBottom")]
        RightBottom
    }

    public class Callout : Control
    {
        protected override string ControlName => "callout";

        IList<Control> _controls = new List<Control>();

        public IList<Control> Controls
        {
            get { return _controls; }
            set { _controls = value; }
        }

        public EventHandler OnDismiss
        {
            get { return GetEventHandler("dismiss"); }
            set { SetEventHandler("dismiss", value); }
        }

        public string Target
        {
            get { return GetAttr("target"); }
            set { SetAttr("target", value); }
        }

        public CalloutPosition Position
        {
            get { return GetEnumAttr<CalloutPosition>("position"); }
            set { SetEnumAttr("position", value); }
        }

        public int Gap
        {
            get { return GetIntAttr("gap"); }
            set { SetIntAttr("gap", value); }
        }

        public bool Beak
        {
            get { return GetBoolAttr("beak"); }
            set { SetBoolAttr("beak", value); }
        }

        public int BeakWidth
        {
            get { return GetIntAttr("beakWidth"); }
            set { SetIntAttr("beakWidth", value); }
        }

        public int PagePadding
        {
            get { return GetIntAttr("pagePadding"); }
            set { SetIntAttr("pagePadding", value); }
        }

        public bool Focus
        {
            get { return GetBoolAttr("focus"); }
            set { SetBoolAttr("focus", value); }
        }

        public bool Cover
        {
            get { return GetBoolAttr("cover"); }
            set { SetBoolAttr("cover", value); }
        }

        protected override IEnumerable<Control> GetChildren()
        {
            return _controls;
        }
    }
}

namespace Pglet.Controls
{
    public class Progress : Control
    {
        protected override string ControlName => "progress";

        public int? Value
        {
            get { return GetNullableIntAttr("value"); }
            set { SetNullableIntAttr("value", value); }
        }

        public int? BarHeight
        {
            get { return GetNullableIntAttr("barheight"); }
            set { SetNullableIntAttr("barheight", value); }
        }

        public string Label
        {
            get { return GetAttr("label"); }
            set { SetAttr("label", value); }
        }

        public string Description
        {
            get { return GetAttr("description"); }
            set { SetAttr("description", value); }
        }
    }
}

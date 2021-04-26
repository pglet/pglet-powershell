namespace Pglet.Controls
{
    public class Spinner : Control
    {
        protected override string ControlName => "spinner";

        public string Label
        {
            get { return GetAttr("label"); }
            set { SetAttr("label", value); }
        }

        public SpinnerSize Size
        {
            get { return GetEnumAttr<SpinnerSize>("size"); }
            set { SetEnumAttr("size", value); }
        }

        public SpinnerLabelPosition LabelPosition
        {
            get { return GetEnumAttr<SpinnerLabelPosition>("labelPosition"); }
            set { SetEnumAttr("labelPosition", value); }
        }
    }
}

using System.Collections.Generic;

namespace Pglet.Controls
{
    public class PieChart : Control
    {
        protected override string ControlName => "piechart";

        readonly PieChartData _data = new();

        public IList<PieChartDataPoint> Points
        {
            get { return _data.Points; }
            set { _data.Points = value; }
        }

        public bool Legend
        {
            get { return GetBoolAttr("legend"); }
            set { SetBoolAttr("legend", value); }
        }

        public bool Tooltips
        {
            get { return GetBoolAttr("tooltips"); }
            set { SetBoolAttr("tooltips", value); }
        }

        object _innerValue;
        public object InnerValue
        {
            get { return _innerValue; }
            set { SetAttr("innerValue", value); _innerValue = value; }
        }

        public int? InnerRadius
        {
            get { return GetNullableIntAttr("innerRadius"); }
            set { SetNullableIntAttr("innerRadius", value); }
        }

        protected override IEnumerable<Control> GetChildren()
        {
            return new Control[] { _data };
        }
    }
}

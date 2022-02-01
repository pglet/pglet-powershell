using System.Management.Automation;

namespace Pglet.PowerShell
{
    [Cmdlet(VerbsCommon.New, "PgletVerticalBarChart")]
    [OutputType(typeof(VerticalBarChart))]
    public class NewPgletVerticalBarChartCommand : NewControlCmdletBase
    {
        [Parameter(Mandatory = false)]
        public VerticalBarChartDataPoint[] Points { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter Tooltips { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter Legend { get; set; }

        [Parameter(Mandatory = false)]
        public int? BarWidth { get; set; }

        [Parameter(Mandatory = false)]
        public string[] Colors { get; set; }

        [Parameter(Mandatory = false)]
        public object YMin { get; set; }

        [Parameter(Mandatory = false)]
        public object YMax { get; set; }

        [Parameter(Mandatory = false)]
        public int? YTicks { get; set; }

        [Parameter(Mandatory = false)]
        public string YFormat { get; set; }

        [Parameter(Mandatory = false)]
        public VerticalBarChartXType? XType { get; set; }

        protected override void ProcessRecord()
        {
            var chart = new VerticalBarChart
            {
                YMin = YMin,
                YMax = YMax,
                YFormat = YFormat,
                Colors = Colors
            };

            SetControlProps(chart);

            if (Points != null)
            {
                foreach (var point in Points)
                {
                    chart.Points.Add(point);
                }
            }

            if (Tooltips.IsPresent)
            {
                chart.Tooltips = Tooltips.ToBool();
            }

            if (Legend.IsPresent)
            {
                chart.Legend = Legend.ToBool();
            }

            if (BarWidth.HasValue)
            {
                chart.BarWidth = BarWidth.Value;
            }

            if (YTicks.HasValue)
            {
                chart.YTicks = YTicks.Value;
            }

            if (XType.HasValue)
            {
                chart.XType = XType.Value;
            }

            WriteObject(chart);
        }
    }
}

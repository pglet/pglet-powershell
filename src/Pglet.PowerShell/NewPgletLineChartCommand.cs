using Pglet.Controls;
using System.Management.Automation;

namespace Pglet.PowerShell
{
    [Cmdlet(VerbsCommon.New, "PgletLineChart")]
    [OutputType(typeof(LineChart))]
    public class NewPgletLineChartCommand : NewControlCmdletBase
    {
        [Parameter(Mandatory = false)]
        public LineChartData[] Lines { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter Tooltips { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter Legend { get; set; }

        [Parameter(Mandatory = false)]
        public int? StrokeWidth { get; set; }

        [Parameter(Mandatory = false)]
        public object YMin { get; set; }

        [Parameter(Mandatory = false)]
        public object YMax { get; set; }

        [Parameter(Mandatory = false)]
        public int? YTicks { get; set; }

        [Parameter(Mandatory = false)]
        public string YFormat { get; set; }

        [Parameter(Mandatory = false)]
        public LineChartXType? XType { get; set; }

        protected override void ProcessRecord()
        {
            var chart = new LineChart
            {
                YMin = YMin,
                YMax = YMax,
                YFormat = YFormat
            };

            SetControlProps(chart);

            if (Lines != null)
            {
                foreach (var line in Lines)
                {
                    chart.Lines.Add(line);
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

            if (StrokeWidth.HasValue)
            {
                chart.StrokeWidth = StrokeWidth.Value;
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

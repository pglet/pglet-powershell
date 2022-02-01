using System.Management.Automation;

namespace Pglet.PowerShell
{
    [Cmdlet(VerbsCommon.New, "PgletBarChart")]
    [OutputType(typeof(BarChart))]
    public class NewPgletBarChartCommand : NewControlCmdletBase
    {
        [Parameter(Mandatory = false)]
        public BarChartDataPoint[] Points { get; set; }

        [Parameter(Mandatory = false)]
        public BarChartDataMode? DataMode { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter Tooltips { get; set; }

        protected override void ProcessRecord()
        {
            var chart = new BarChart();
            if (DataMode.HasValue)
            {
                chart.DataMode = DataMode.Value;
            }

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

            SetControlProps(chart);

            WriteObject(chart);
        }
    }
}

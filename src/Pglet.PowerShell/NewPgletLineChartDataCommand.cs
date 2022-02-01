using System.Management.Automation;

namespace Pglet.PowerShell
{
    [Cmdlet(VerbsCommon.New, "PgletLineChartData")]
    [OutputType(typeof(LineChartData))]
    public class NewPgletLineChartDataCommand : NewControlCmdletBase
    {
        [Parameter(Mandatory = false)]
        public LineChartDataPoint[] Points { get; set; }

        [Parameter(Mandatory = false)]
        public string Legend { get; set; }

        [Parameter(Mandatory = false)]
        public string Color { get; set; }

        protected override void ProcessRecord()
        {
            var data = new LineChartData
            {
                Legend = Legend,
                Color = Color
            };

            if (Points != null)
            {
                foreach (var point in Points)
                {
                    data.Points.Add(point);
                }
            }

            SetControlProps(data);

            WriteObject(data);
        }
    }
}

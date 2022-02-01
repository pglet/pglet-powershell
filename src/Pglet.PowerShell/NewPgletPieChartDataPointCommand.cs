using System.Management.Automation;

namespace Pglet.PowerShell
{
    [Cmdlet(VerbsCommon.New, "PgletPieChartDataPoint")]
    [OutputType(typeof(PieChartDataPoint))]
    public class NewPgletPieChartDataPointCommand : NewControlCmdletBase
    {
        [Parameter(Mandatory = false)]
        public object Value { get; set; }

        [Parameter(Mandatory = false)]
        public string Legend { get; set; }

        [Parameter(Mandatory = false)]
        public string Color { get; set; }

        [Parameter(Mandatory = false)]
        public string Tooltip { get; set; }

        protected override void ProcessRecord()
        {
            var p = new PieChartDataPoint
            {
                Value = Value,
                Legend = Legend,
                Color = Color,
                Tooltip = Tooltip
            };

            SetControlProps(p);

            WriteObject(p);
        }
    }
}

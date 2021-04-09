using Pglet.Controls;
using Pglet.PowerShell.Controls;
using System.Management.Automation;

namespace Pglet.PowerShell
{
    [Cmdlet(VerbsCommon.New, "PgletLineChartDataPoint")]
    [OutputType(typeof(LineChartDataPoint))]
    public class NewPgletLineChartDataPointCommand : NewControlCmdletBase
    {
        [Parameter(Mandatory = false)]
        public object X { get; set; }

        [Parameter(Mandatory = false)]
        public object Y { get; set; }

        [Parameter(Mandatory = false)]
        public object Tick { get; set; }

        [Parameter(Mandatory = false)]
        public string Legend { get; set; }

        [Parameter(Mandatory = false)]
        public string XTooltip { get; set; }

        [Parameter(Mandatory = false)]
        public string YTooltip { get; set; }

        protected override void ProcessRecord()
        {
            var p = new LineChartDataPoint
            {
                X = X,
                Y = Y,
                Tick = Tick,
                Legend = Legend,
                XTooltip = XTooltip,
                YTooltip = YTooltip
            };

            SetControlProps(p);

            WriteObject(p);
        }
    }
}

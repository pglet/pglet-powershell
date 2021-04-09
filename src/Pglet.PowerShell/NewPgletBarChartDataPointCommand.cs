using Pglet.Controls;
using Pglet.PowerShell.Controls;
using System.Management.Automation;

namespace Pglet.PowerShell
{
    [Cmdlet(VerbsCommon.New, "PgletBarChartDataPoint")]
    [OutputType(typeof(BarChartDataPoint))]
    public class NewPgletBarChartDataPointCommand : NewControlCmdletBase
    {
        [Parameter(Mandatory = false)]
        public object X { get; set; }

        [Parameter(Mandatory = false)]
        public object Y { get; set; }

        [Parameter(Mandatory = false)]
        public string Legend { get; set; }

        [Parameter(Mandatory = false)]
        public string Color { get; set; }

        [Parameter(Mandatory = false)]
        public string XTooltip { get; set; }

        [Parameter(Mandatory = false)]
        public string YTooltip { get; set; }

        protected override void ProcessRecord()
        {
            var p = new BarChartDataPoint
            {
                X = X,
                Y = Y,
                Legend = Legend,
                Color = Color,
                XTooltip = XTooltip,
                YTooltip = YTooltip
            };

            SetControlProps(p);

            WriteObject(p);
        }
    }
}

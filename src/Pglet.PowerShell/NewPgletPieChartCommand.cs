﻿using System.Management.Automation;

namespace Pglet.PowerShell
{
    [Cmdlet(VerbsCommon.New, "PgletPieChart")]
    [OutputType(typeof(PieChart))]
    public class NewPgletPieChartCommand : NewControlCmdletBase
    {
        [Parameter(Mandatory = false)]
        public PieChartDataPoint[] Points { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter Legend { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter Tooltips { get; set; }

        [Parameter(Mandatory = false)]
        public object InnerValue { get; set; }

        [Parameter(Mandatory = false)]
        public int? InnerRadius { get; set; }

        protected override void ProcessRecord()
        {
            var chart = new PieChart
            {
                InnerRadius = InnerRadius,
                InnerValue = InnerValue
            };

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

            SetControlProps(chart);

            WriteObject(chart);
        }
    }
}

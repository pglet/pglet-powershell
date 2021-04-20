using Pglet.Controls;
using Pglet.PowerShell.Controls;
using System.Management.Automation;

namespace Pglet.PowerShell
{
    [Cmdlet(VerbsCommon.New, "PgletGrid")]
    [OutputType(typeof(PsGrid))]
    public class NewPgletGridCommand : NewControlCmdletBase
    {
        [Parameter(Mandatory = false)]
        public PsGridColumn[] Columns { get; set; }

        [Parameter(Mandatory = false)]
        public object[] Items { get; set; }

        [Parameter(Mandatory = false)]
        public ScriptBlock OnSelect { get; set; }

        [Parameter(Mandatory = false)]
        public ScriptBlock OnItemInvoke { get; set; }

        [Parameter(Mandatory = false)]
        public GridSelectionMode? SelectionMode { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter Compact { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter HeaderVisible { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter PreserveSelection { get; set; }

        [Parameter(Mandatory = false)]
        public int? ShimmerLines { get; set; }

        protected override void ProcessRecord()
        {
            var ctl = new PsGrid
            {
                OnSelect = OnSelect,
                OnItemInvoke = OnItemInvoke
            };

            SetControlProps(ctl);

            if (SelectionMode.HasValue)
            {
                ctl.SelectionMode = SelectionMode.Value;
            }

            if (ShimmerLines.HasValue)
            {
                ctl.ShimmerLines = ShimmerLines.Value;
            }

            if (Compact.IsPresent)
            {
                ctl.Compact = Compact.ToBool();
            }

            if (HeaderVisible.IsPresent)
            {
                ctl.HeaderVisible = HeaderVisible.ToBool();
            }

            if (PreserveSelection.IsPresent)
            {
                ctl.PreserveSelection = PreserveSelection.ToBool();
            }

            if (Columns != null)
            {
                foreach (var column in Columns)
                {
                    ctl.Columns.Add(column);
                }
            }

            if (Items != null)
            {
                foreach (var item in Items)
                {
                    ctl.Items.Add(item);
                }
            }

            WriteObject(ctl);
        }
    }
}

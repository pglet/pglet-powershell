using Pglet.Controls;
using Pglet.PowerShell.Controls;
using System.Management.Automation;

namespace Pglet.PowerShell
{
    [Cmdlet(VerbsCommon.New, "PgletGridColumn")]
    [OutputType(typeof(PsGridColumn))]
    public class NewPgletGridColumnCommand : NewControlCmdletBase
    {
        [Parameter(Mandatory = false)]
        public Control[] TemplateControls { get; set; }

        [Parameter(Mandatory = false)]
        public string Name { get; set; }

        [Parameter(Mandatory = false)]
        public string Icon { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter IconOnly { get; set; }

        [Parameter(Mandatory = false)]
        public string FieldName { get; set; }

        [Parameter(Mandatory = false)]
        public string Sortable { get; set; }

        [Parameter(Mandatory = false)]
        public string SortField { get; set; }

        [Parameter(Mandatory = false)]
        public string Sorted { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter Resizable { get; set; }

        [Parameter(Mandatory = false)]
        public int? MinWidth { get; set; }

        [Parameter(Mandatory = false)]
        public int? MaxWidth { get; set; }

        [Parameter(Mandatory = false)]
        public ScriptBlock OnClick { get; set; }

        protected override void ProcessRecord()
        {
            var ctl = new PsGridColumn
            {
                OnClick = OnClick,
                Name = Name,
                Icon = Icon,
                FieldName = FieldName,
                Sortable = Sortable,
                SortField = SortField,
                Sorted = Sorted
            };

            SetControlProps(ctl);

            if (MinWidth.HasValue)
            {
                ctl.MinWidth = MinWidth.Value;
            }

            if (MaxWidth.HasValue)
            {
                ctl.MaxWidth = MaxWidth.Value;
            }

            if (IconOnly.IsPresent)
            {
                ctl.IconOnly = IconOnly.ToBool();
            }

            if (Resizable.IsPresent)
            {
                ctl.Resizable = Resizable.ToBool();
            }

            if (TemplateControls != null)
            {
                foreach (var control in TemplateControls)
                {
                    ctl.TemplateControls.Add(control);
                }
            }

            WriteObject(ctl);
        }
    }
}

using Pglet.Controls;
using Pglet.PowerShell.Controls;
using System.Management.Automation;

namespace Pglet.PowerShell
{
    [Cmdlet(VerbsCommon.New, "PgletTabs")]
    [OutputType(typeof(PsTabs))]
    public class NewPgletTabsCommand : NewControlCmdletBase
    {
        [Parameter(Mandatory = false)]
        public string Value { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter Solid { get; set; }

        [Parameter(Mandatory = false)]
        public ScriptBlock OnChange { get; set; }

        [Parameter(Mandatory = false)]
        public Tab[] TabItems { get; set; }

        protected override void ProcessRecord()
        {
            var ctl = new PsTabs
            {
                Value = Value,
                OnChange = OnChange
            };

            SetControlProps(ctl);

            if (Solid.IsPresent)
            {
                ctl.Solid = Solid.ToBool();
            }

            if (TabItems != null)
            {
                foreach (var tab in TabItems)
                {
                    ctl.TabItems.Add(tab);
                }
            }

            WriteObject(ctl);
        }
    }
}

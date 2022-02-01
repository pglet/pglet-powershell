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
            var control = new PsTabs();

            SetControlProps(control);

            control.Value = Value;
            control.OnChange = OnChange;

            if (Solid.IsPresent)
            {
                control.Solid = Solid.ToBool();
            }

            if (TabItems != null)
            {
                foreach (var tab in TabItems)
                {
                    control.TabItems.Add(tab);
                }
            }

            WriteObject(control);
        }
    }
}

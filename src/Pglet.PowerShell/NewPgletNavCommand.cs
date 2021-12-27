using Pglet.Controls;
using Pglet.PowerShell.Controls;
using System.Management.Automation;

namespace Pglet.PowerShell
{
    [Cmdlet(VerbsCommon.New, "PgletNav")]
    [OutputType(typeof(PsNav))]
    public class NewPgletNavCommand : NewControlCmdletBase
    {
        [Parameter(Mandatory = false)]
        public string Value { get; set; }

        [Parameter(Mandatory = false)]
        public NavItem[] Items { get; set; }

        [Parameter(Mandatory = false)]
        public ScriptBlock OnChange { get; set; }

        [Parameter(Mandatory = false)]
        public ScriptBlock OnExpand { get; set; }

        [Parameter(Mandatory = false)]
        public ScriptBlock OnCollapse { get; set; }

        protected override void ProcessRecord()
        {
            var control = new PsNav();

            SetControlProps(control);

            control.Value = Value;
            control.OnChange = OnChange;
            control.OnExpand = OnExpand;
            control.OnCollapse = OnCollapse;

            if (Items != null)
            {
                foreach (var item in Items)
                {
                    control.Items.Add(item);
                }
            }

            WriteObject(control);
        }
    }
}

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
            var ctl = new PsNav
            {
                Value = Value,
                OnChange = OnChange,
                OnExpand = OnExpand,
                OnCollapse = OnCollapse
            };

            SetControlProps(ctl);

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

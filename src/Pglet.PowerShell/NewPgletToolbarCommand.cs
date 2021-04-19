using Pglet.Controls;
using Pglet.PowerShell.Controls;
using System.Management.Automation;

namespace Pglet.PowerShell
{
    [Cmdlet(VerbsCommon.New, "PgletToolbar")]
    [OutputType(typeof(Toolbar))]
    public class NewPgletToolbarCommand : NewControlCmdletBase
    {
        [Parameter(Mandatory = false)]
        public SwitchParameter Inverted { get; set; }

        [Parameter(Mandatory = false)]
        public PsMenuItem[] Items { get; set; }

        [Parameter(Mandatory = false)]
        public PsMenuItem[] OverflowItems { get; set; }

        [Parameter(Mandatory = false)]
        public PsMenuItem[] FarItems { get; set; }

        protected override void ProcessRecord()
        {
            var ctl = new Toolbar();

            SetControlProps(ctl);

            if (Inverted.IsPresent)
            {
                ctl.Inverted = Inverted.ToBool();
            }

            if (Items != null)
            {
                foreach (var item in Items)
                {
                    ctl.Items.Add(item);
                }
            }

            if (OverflowItems != null)
            {
                foreach (var item in OverflowItems)
                {
                    ctl.OverflowItems.Add(item);
                }
            }

            if (FarItems != null)
            {
                foreach (var item in FarItems)
                {
                    ctl.FarItems.Add(item);
                }
            }

            WriteObject(ctl);
        }
    }
}

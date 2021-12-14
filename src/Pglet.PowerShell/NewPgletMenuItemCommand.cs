using Pglet.PowerShell.Controls;
using System.Management.Automation;

namespace Pglet.PowerShell
{
    [Cmdlet(VerbsCommon.New, "PgletMenuItem")]
    [OutputType(typeof(PsMenuItem))]
    public class NewPgletMenuItemCommand : NewControlCmdletBase
    {
        [Parameter(Mandatory = false, Position = 0)]
        public string Text { get; set; }

        [Parameter(Mandatory = false)]
        public string SecondaryText { get; set; }

        [Parameter(Mandatory = false)]
        public string Url { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter NewWindow { get; set; }

        [Parameter(Mandatory = false)]
        public string Icon { get; set; }

        [Parameter(Mandatory = false)]
        public string IconColor { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter IconOnly { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter Split { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter Divider { get; set; }

        [Parameter(Mandatory = false)]
        public PsMenuItem[] SubMenuItems { get; set; }

        [Parameter(Mandatory = false)]
        public ScriptBlock OnClick { get; set; }

        protected override void ProcessRecord()
        {
            var ctl = new PsMenuItem
            {
                Text = Text,
                SecondaryText = SecondaryText,
                Url = Url,
                Icon = Icon,
                IconColor = IconColor,
                OnClick = OnClick
            };

            if (NewWindow.IsPresent)
            {
                ctl.NewWindow = NewWindow.ToBool();
            }

            if (IconOnly.IsPresent)
            {
                ctl.IconOnly = IconOnly.ToBool();
            }

            if (Split.IsPresent)
            {
                ctl.Split = Split.ToBool();
            }

            if (Divider.IsPresent)
            {
                ctl.Divider = Divider.ToBool();
            }

            if (SubMenuItems != null)
            {
                foreach (var subMenuItem in SubMenuItems)
                {
                    ctl.SubMenuItems.Add(subMenuItem);
                }
            }

            SetControlProps(ctl);

            WriteObject(ctl);
        }
    }
}

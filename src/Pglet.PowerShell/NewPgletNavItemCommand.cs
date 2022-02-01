using System.Management.Automation;

namespace Pglet.PowerShell
{
    [Cmdlet(VerbsCommon.New, "PgletNavItem")]
    [OutputType(typeof(NavItem))]
    public class NewPgletNavItemCommand : NewControlCmdletBase
    {
        [Parameter(Mandatory = false)]
        public string Key { get; set; }

        [Parameter(Mandatory = false)]
        public string Text { get; set; }

        [Parameter(Mandatory = false)]
        public string Url { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter NewWindow { get; set; }

        [Parameter(Mandatory = false)]
        public string Icon { get; set; }

        [Parameter(Mandatory = false)]
        public string IconColor { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter Expanded { get; set; }

        [Parameter(Mandatory = false)]
        public NavItem[] SubItems { get; set; }

        protected override void ProcessRecord()
        {
            var ctl = new NavItem
            {
                Key = Key,
                Text = Text,
                Url = Url,
                Icon = Icon,
                IconColor = IconColor
            };

            if (NewWindow.IsPresent)
            {
                ctl.NewWindow = NewWindow.ToBool();
            }

            if (Expanded.IsPresent)
            {
                ctl.Expanded = Expanded.ToBool();
            }

            if (SubItems != null)
            {
                foreach (var subItem in SubItems)
                {
                    ctl.SubItems.Add(subItem);
                }
            }

            SetControlProps(ctl);

            WriteObject(ctl);
        }
    }
}

using Pglet.PowerShell.Controls;
using System.Management.Automation;

namespace Pglet.PowerShell
{
    [Cmdlet(VerbsCommon.New, "PgletButton")]
    [OutputType(typeof(PsButton))]
    public class NewPgletButtonCommand : NewControlCmdletBase
    {
        [Parameter(Mandatory = false)]
        public SwitchParameter Primary { get; set; }
        
        [Parameter(Mandatory = false)]
        public SwitchParameter Compound { get; set; }
        
        [Parameter(Mandatory = false)]
        public SwitchParameter Action { get; set; }
        
        [Parameter(Mandatory = false)]
        public SwitchParameter Toolbar { get; set; }
        
        [Parameter(Mandatory = false)]
        public SwitchParameter Split { get; set; }

        [Parameter(Mandatory = false)]
        public string Text { get; set; }

        [Parameter(Mandatory = false)]
        public string SecondaryText { get; set; }

        [Parameter(Mandatory = false)]
        public string Url { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter NewWindow { get; set; }

        [Parameter(Mandatory = false)]
        public string Title { get; set; }

        [Parameter(Mandatory = false)]
        public string Icon { get; set; }

        [Parameter(Mandatory = false)]
        public string IconColor { get; set; }

        [Parameter(Mandatory = false)]
        public PsMenuItem[] MenuItems { get; set; }

        [Parameter(Mandatory = false)]
        public ScriptBlock OnClick { get; set; }

        protected override void ProcessRecord()
        {
            var ctl = new PsButton
            {
                Text = Text,
                SecondaryText = SecondaryText,
                Url = Url,
                Title = Title,
                Icon = Icon,
                IconColor = IconColor,
                OnClick = OnClick
            };

            if (NewWindow.IsPresent)
            {
                ctl.NewWindow = NewWindow.ToBool();
            }

            if (Primary.IsPresent)
            {
                ctl.Primary = Primary.ToBool();
            }

            if (Compound.IsPresent)
            {
                ctl.Compound = Compound.ToBool();
            }

            if (Action.IsPresent)
            {
                ctl.Action = Action.ToBool();
            }

            if (Toolbar.IsPresent)
            {
                ctl.Toolbar = Toolbar.ToBool();
            }

            if (Split.IsPresent)
            {
                ctl.Split = Split.ToBool();
            }

            SetControlProps(ctl);

            WriteObject(ctl);
        }
    }
}

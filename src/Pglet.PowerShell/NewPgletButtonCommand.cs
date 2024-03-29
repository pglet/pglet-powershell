﻿using Pglet.PowerShell.Controls;
using System.Management.Automation;

namespace Pglet.PowerShell
{
    [Cmdlet(VerbsCommon.New, "PgletButton")]
    [OutputType(typeof(PsButton))]
    public class NewPgletButtonCommand : NewControlCmdletBase
    {
        [Parameter(Mandatory = false, Position = 0)]
        public string Text { get; set; }

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
            var ctl = new PsButton();

            SetControlProps(ctl);

            ctl.Text = Text;
            ctl.SecondaryText = SecondaryText;
            ctl.Url = Url;
            ctl.Title = Title;
            ctl.Icon = Icon;
            ctl.IconColor = IconColor;
            ctl.OnClick = OnClick;

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

            if (MenuItems != null)
            {
                foreach (var menuItem in MenuItems)
                {
                    ctl.MenuItems.Add(menuItem);
                }
            }

            WriteObject(ctl);
        }
    }
}

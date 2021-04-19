using Pglet.Controls;
using Pglet.PowerShell.Controls;
using System.Management.Automation;

namespace Pglet.PowerShell
{
    [Cmdlet(VerbsCommon.New, "PgletLink")]
    [OutputType(typeof(PsLink))]
    public class NewPgletLinkCommand : NewControlCmdletBase
    {
        [Parameter(Mandatory = false)]
        public string Value { get; set; }

        [Parameter(Mandatory = false)]
        public string Url { get; set; }

        [Parameter(Mandatory = false)]
        public string Title { get; set; }

        [Parameter(Mandatory = false)]
        public string Size { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter Bold { get; set; }
        
        [Parameter(Mandatory = false)]
        public SwitchParameter Italic { get; set; }
        
        [Parameter(Mandatory = false)]
        public SwitchParameter Pre { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter NewWindow { get; set; }

        [Parameter(Mandatory = false)]
        public TextAlign? Align { get; set; }

        [Parameter(Mandatory = false)]
        public Control[] Controls { get; set; }

        [Parameter(Mandatory = false)]
        public ScriptBlock OnClick { get; set; }

        protected override void ProcessRecord()
        {
            var ctl = new PsLink
            {
                Value = Value,
                Url = Url,
                Title = Title,
                Size = Size,
                OnClick = OnClick
            };

            if (NewWindow.IsPresent)
            {
                ctl.NewWindow = NewWindow.ToBool();
            }

            if (Bold.IsPresent)
            {
                ctl.Bold = Bold.ToBool();
            }

            if (Italic.IsPresent)
            {
                ctl.Italic = Italic.ToBool();
            }

            if (Pre.IsPresent)
            {
                ctl.Pre = Pre.ToBool();
            }

            if (Align.HasValue)
            {
                ctl.Align = Align.Value;
            }

            SetControlProps(ctl);

            if (Controls != null)
            {
                foreach (var control in Controls)
                {
                    ctl.Controls.Add(control);
                }
            }

            WriteObject(ctl);
        }
    }
}

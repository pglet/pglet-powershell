using Pglet.Controls;
using System.Management.Automation;

namespace Pglet.PowerShell
{
    [Cmdlet(VerbsCommon.New, "PgletImage")]
    [OutputType(typeof(Image))]
    public class NewPgletImageCommand : NewControlCmdletBase
    {
        [Parameter(Mandatory = false, Position = 0)]
        public string Src { get; set; }

        [Parameter(Mandatory = false)]
        public string Alt { get; set; }

        [Parameter(Mandatory = false)]
        public string Title { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter MaximizeFrame { get; set; }

        protected override void ProcessRecord()
        {
            var control = new Image
            {
                Src = Src,
                Alt = Alt,
                Title = Title
            };

            SetControlProps(control);

            if (MaximizeFrame.IsPresent)
            {
                control.MaximizeFrame = MaximizeFrame.ToBool();
            }

            WriteObject(control);
        }
    }
}

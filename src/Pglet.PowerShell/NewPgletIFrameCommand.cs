using Pglet.Controls;
using System.Management.Automation;

namespace Pglet.PowerShell
{
    [Cmdlet(VerbsCommon.New, "PgletIFrame")]
    [OutputType(typeof(IFrame))]
    public class NewPgletIFrameCommand : NewControlCmdletBase
    {
        [Parameter(Mandatory = false, Position = 0)]
        public string Src { get; set; }

        [Parameter(Mandatory = false)]
        public string Title { get; set; }

        [Parameter(Mandatory = false)]
        public string Border { get; set; }

        protected override void ProcessRecord()
        {
            var control = new IFrame
            {
                Src = Src,
                Title = Title,
                Border = Border
            };

            SetControlProps(control);

            WriteObject(control);
        }
    }
}

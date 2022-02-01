using System.Management.Automation;

namespace Pglet.PowerShell
{
    [Cmdlet(VerbsCommon.New, "PgletIcon")]
    [OutputType(typeof(Icon))]
    public class NewPgletIconCommand : NewControlCmdletBase
    {
        [Parameter(Mandatory = false, Position = 0)]
        public string Name { get; set; }

        [Parameter(Mandatory = false)]
        public string Color { get; set; }

        [Parameter(Mandatory = false)]
        public string Size { get; set; }

        protected override void ProcessRecord()
        {
            var control = new Icon
            {
                Name = Name,
                Color = Color,
                Size = Size
            };

            SetControlProps(control);

            WriteObject(control);
        }
    }
}

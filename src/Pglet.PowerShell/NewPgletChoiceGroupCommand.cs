using Pglet.PowerShell.Controls;
using System.Management.Automation;

namespace Pglet.PowerShell
{
    [Cmdlet(VerbsCommon.New, "PgletChoiceGroup")]
    [OutputType(typeof(PsChoiceGroup))]
    public class NewPgletChoiceGroupCommand : NewControlCmdletBase
    {
        [Parameter(Mandatory = false, Position = 0)]
        public string Label { get; set; }

        [Parameter(Mandatory = false)]
        public string Value { get; set; }

        [Parameter(Mandatory = false)]
        public ScriptBlock OnChange { get; set; }

        [Parameter(Mandatory = false)]
        public ChoiceGroupOption[] Options { get; set; }

        protected override void ProcessRecord()
        {
            var ctl = new PsChoiceGroup();

            SetControlProps(ctl);

            ctl.Label = Label;
            ctl.Value = Value;
            ctl.OnChange = OnChange;

            if (Options != null)
            {
                foreach (var option in Options)
                {
                    ctl.Options.Add(option);
                }
            }

            WriteObject(ctl);
        }
    }
}

using Pglet.Controls;
using System.Collections.Generic;
using System.Management.Automation;

namespace Pglet.PowerShell
{
    [Cmdlet(VerbsCommon.New, "PgletButton")]
    [OutputType(typeof(Page))]
    public class NewPgletButtonCommand : PSCmdlet
    {
        public class PsButton : Button, IPsEventControl
        {
            readonly Dictionary<string, ScriptBlock> _psEvents = new();

            public new ScriptBlock OnClick
            {
                get
                {
                    return _psEvents.ContainsKey("click") ? _psEvents["click"] : null;
                }
                set
                {
                    _psEvents["click"] = value;
                }
            }

            public ScriptBlock GetEventHandlerScript(string eventName)
            {
                return _psEvents.ContainsKey(eventName) ? _psEvents[eventName] : null;
            }
        }

        [Parameter(Mandatory = false)]
        public string Id { get; set; }

        [Parameter(Mandatory = false)]
        public string Text { get; set; }

        [Parameter(Mandatory = false)]
        public ScriptBlock OnClick { get; set; }

        protected override void ProcessRecord()
        {
            var ctl = new PsButton
            {
                Id = Id,
                Text = Text,
                OnClick = OnClick
            };

            WriteObject(ctl);
        }
    }
}

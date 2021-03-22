﻿using Pglet.Controls;
using Pglet.PowerShell.Controls;
using System.Management.Automation;

namespace Pglet.PowerShell.Controls
{
    [Cmdlet(VerbsCommon.New, "PgletText")]
    [OutputType(typeof(Page))]
    public class NewPgletTextCommand : PSCmdlet
    {
        [Parameter(Mandatory = false)]
        public string Id { get; set; }

        [Parameter(Mandatory = false)]
        public string Value { get; set; }

        protected override void ProcessRecord()
        {
            var ctl = new Text();
            ctl.Id = Id;
            ctl.Value = Value;

            WriteObject(ctl);
        }
    }
}
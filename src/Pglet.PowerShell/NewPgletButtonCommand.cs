﻿using System;
using System.Management.Automation;
using System.Threading;
using System.Threading.Tasks;

namespace Pglet.PowerShell
{
    [Cmdlet(VerbsCommon.New, "PgletButton")]
    [OutputType(typeof(Page))]
    public class NewPgletButtonCommand : PSCmdlet
    {
        [Parameter(Mandatory = false)]
        public string Id { get; set; }

        [Parameter(Mandatory = false)]
        public ScriptBlock OnClick { get; set; }

        protected override void EndProcessing()
        {
            WriteObject("Button!");
        }
    }
}

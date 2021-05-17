using System;
using System.Management.Automation;
using System.Threading;

namespace Pglet.PowerShell
{
    [Cmdlet(VerbsCommon.Show, "PgletSignin")]
    [OutputType(typeof(bool))]
    public class ShowPgletSigninCommand : PSCmdlet
    {
        readonly CancellationTokenSource _cancellationSource = new();

        [Parameter(Mandatory = false, Position = 0, HelpMessage = "Page object.")]
        public Page Page { get; set; }

        [Parameter(Mandatory = false)]
        public string[] AuthProviders { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter AuthGroups { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter AllowDismiss { get; set; }

        protected override void ProcessRecord()
        {
            var page = Page;
            if (page == null)
            {
                page = SessionState.PSVariable.Get(Constants.PGLET_PAGE).Value as Page;
            }

            if (page == null)
            {
                throw new Exception("There are no active Pglet connections.");
            }

            string authProviders = "*";
            if (AuthProviders != null && AuthProviders.Length > 0)
            {
                authProviders = string.Join(",", AuthProviders);
            }

            var result = page.ShowSignin(authProviders, AuthGroups.IsPresent, AllowDismiss.IsPresent, _cancellationSource.Token);
            WriteObject(result);
        }

        protected override void StopProcessing()
        {
            _cancellationSource.Cancel();
            base.StopProcessing();
        }
    }
}

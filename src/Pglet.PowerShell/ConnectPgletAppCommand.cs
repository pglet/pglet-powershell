using Pglet.PowerShell.Controls;
using System;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Threading;
using System.Threading.Tasks;

namespace Pglet.PowerShell
{
    [Cmdlet(VerbsCommunications.Connect, "PgletApp")]
    public class ConnectPgletAppCommand : PSCmdlet
    {
        readonly CancellationTokenSource _cancellationSource = new();

        [Parameter(Mandatory = false, Position = 0, HelpMessage = "The name of Pglet page.")]
        public string Name { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "A handler script block for a new user session.")]
        public ScriptBlock ScriptBlock { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Makes the page available as public at pglet.io service or a self-hosted Pglet server.")]
        public SwitchParameter Web { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Do not open browser window.")]
        public SwitchParameter NoWindow { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Connects to the page on a self-hosted Pglet server.")]
        public string Server { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Authentication token for pglet.io service or a self-hosted Pglet server.")]
        public string Token { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Interval in milliseconds between 'tick' events; disabled if not specified.")]
        public int? Ticker { get; set; }

        protected override void ProcessRecord()
        {
            var pgletModulePath = this.MyInvocation.MyCommand.Module.Path.Replace(".psm1", ".psd1");

            void pageCreated(string pageUrl)
            {
                Host.UI.WriteLine(pageUrl);
            }

            PgletClient.ServeApp((page) =>
            {
                return Task.Run(() =>
                {
                    using (var runspace = RunspaceFactory.CreateRunspace())
                    {
                        using (var ps = System.Management.Automation.PowerShell.Create())
                        {
                            using CancellationTokenRegistration ctr = _cancellationSource.Token.Register(() => ps.Stop());

                            try
                            {
                                ps.Runspace = runspace;
                                runspace.Open();
                                runspace.SessionStateProxy.PSVariable.Set(new PSVariable(Constants.PGLET_PAGE, page, ScopedItemOptions.Private));
                                ps.AddScript($"Import-Module '{pgletModulePath}'");
                                ps.AddScript(ScriptBlock.ToString());
                                ps.AddScript("\nSwitch-PgletEvents");
                                ps.Invoke();
                            }
                            catch (RuntimeException ex)
                            {
                                Host.UI.WriteErrorLine(ex.ErrorRecord.ToString() + "\n" + ex.ErrorRecord.InvocationInfo.PositionMessage);
                                throw;
                            }
                        }
                    }
                });
            },
            cancellationToken: _cancellationSource.Token, name: Name, web: Web.ToBool(), noWindow: NoWindow.ToBool(),
                server: Server, token: Token, ticker: Ticker.HasValue ? Ticker.Value : 0,
                createPage: (conn, pageUrl) => new PsPage(conn, pageUrl), pageCreated: pageCreated).Wait();
        }

        protected override void StopProcessing()
        {
            _cancellationSource.Cancel();
            base.StopProcessing();
        }
    }
}

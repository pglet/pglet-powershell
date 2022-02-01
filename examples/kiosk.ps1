Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

Connect-PgletPage -Name "index" -NoWindow

Invoke-Pglet "clean page"

Invoke-Pglet "add
text value='IT Kiosk' size=xxlarge
tabs margin=10
  tab text='New hire'
    stack
      button compound id=createAccount text='Create AD & O365 Account' secondaryText='Create Active Directory and Office 365 account.'
      button compound id=sendEmail text='Send IT Welcome Email' secondaryText='Send a welcome email to a new hire.'
      button compound id=sendCreds text='Send Credentials to Manager' secondaryText='Send O365 and AD passwords in Pwpush links to their manager.'
  tab text='Termination'
    stack
      button compound id=disableAccount text='Disable AD Account' secondaryText='Type a username to disable AD user account.'
      button compound id=disableComputer text='Disable Computer Object' secondaryText='Type computer name to disable computer object.'
"

# Event loop
while($true) {
    $e = Wait-PgletEvent $pageID
    if ($e.name -eq 'click') {
      Write-Host "Button clicked: $($e.target)"
    }
}
Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

Connect-PgletPage -Name "index" -NoWindow

Invoke-Pglet "clean page"

Invoke-Pglet "add
  stack
    nav
      item text='Group 1'
        item expanded text='New'
          item key='email' text='Email message' icon='Mail'
          item key='calendar' text='Calendar event' icon='Calendar' iconColor=salmon
      item text='Group 2' collapsed
        item disabled key=share text='Share' icon='Share'
        item key=twitter text='Share to Twitter'
"

# while($true) {
#     Wait-PgletEvent $pageID
# }
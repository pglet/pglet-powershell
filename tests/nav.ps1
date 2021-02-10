Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

Connect-PgletPage -Name "index" -NoWindow

Invoke-Pglet "clean page"

Invoke-Pglet "add
text value='Nav with groups' size=large
nav
  item text='Group 1'
    item expanded text='New'
      item key='email' text='Email message' icon='Mail'
      item key='calendar' text='Calendar event' icon='Calendar' iconColor=salmon
  item text='Group 2' collapsed
    item disabled key=share text='Share' icon='Share'
    item key=twitter text='Share to Twitter'

text value='Nav without groups' size=large
nav
  item
    item expanded text='New'
      item key='email' text='Email message' icon='Mail'
      item key='calendar' text='Calendar event' icon='Calendar'
    item text=Share
      item key=facebook text='Share on Facebook' icon='Share'
      item key=twitter text='Share to Twitter' icon='Share'
"

# while($true) {
#     Wait-PgletEvent $pageID
# }
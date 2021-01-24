Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

Connect-PgletPage -Name "index" -NoWindow

Invoke-Pglet "clean page"
Invoke-Pglet "set page horizontalAlign='stretch'"

Invoke-Pglet "add
toolbar
  item text='New' icon='Add'
    item text='Email message' icon='Mail'
    item text='Calendar event' icon='Calendar'
  item id=share text='Share' icon='Share' split
    item text='Share to Twitter' data='sharetotwitter'
    item text='Share to Facebook' data='sharetofacebook'
    item text='Share to Somewhere' disabled
    item text='Share to Email' data='sharetoemail'
      item text='Share to Outlook'
      item text='Share to Gmail'
  item text='To to Google' icon='Globe' url='https://google.com' newWindow secondaryText='New window'
  overflow
    item text='Item 1'
    item text='Item 2'
  far
    item text='Grid view' icon=Tiles iconOnly
    item text=Info icon=Info iconOnly
"

# while($true) {
#     Wait-PgletEvent $pageID
# }
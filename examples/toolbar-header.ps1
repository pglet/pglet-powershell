Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

Connect-PgletPage -Name "index" -NoWindow

Invoke-Pglet "clean page"
Invoke-Pglet "set page padding=0 horizontalAlign='stretch' themePrimaryColor='' themeTextColor='' themeBackgroundColor=''"

Invoke-Pglet "add
stack bgcolor=themeDarkAlt horizontal verticalAlign=center horizontalAlign='stretch'
  link width=150px url='#' align=center
    text value='LogoPogo' size=large color='#fff'
  stack id=stack1 width='100%' horizontalAlign='stretch'
    toolbar inverted height=40
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
        item id=grid text='Grid view' icon=Tiles iconOnly checked
        item text=Info icon=Info iconOnly
stack horizontalAlign=start
  button id=showCallout text='Show callout'
callout id=callout1 target='stack1:grid' focus=true visible=false
  stack padding=20 width=300
    text value='Choose your style' size=xlarge
    choicegroup
      option key=red
      option key=green
      option key=blue
    stack horizontal
      button id=yes primary text=Yes
      button id=no text=No
callout id=callout2 target='showCallout' beak=true cover=false position=rightTop pagePadding=0 visible=false
  stack padding=20 width=300 height=100
    text value='That\'s a button callout!' size=xlarge
"

$opened=$false
while($true) {
  $e = Wait-PgletEvent $pageID
  if ($e.target -eq 'stack1:grid' -and -not $opened) {
    Invoke-Pglet "set
      callout1 visible=true
      stack1:grid checked=true"
    $opened = $true
  } elseif (($e.target -eq 'stack1:grid' -and $opened) -or ($e.target -eq 'callout1')) {
    Invoke-Pglet "set
      callout1 visible=false
      stack1:grid checked=false"
    $opened = $false
  } elseif ($e.target -eq 'showCallout') {
    Invoke-Pglet "set callout2 visible=true"
  }
}

# Start-Sleep -s 5

# Invoke-Pglet "set
#   callout1 visible=false
#   stack1:grid checked=false"


Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

Connect-PgletPage -Name "index" -NoWindow

Invoke-Pglet "clean page"

Invoke-Pglet "add
  stack horizontal
    button text='Standard'
    button disabled text='Standard disabled'
  stack horizontal
    button primary text='Primary'
    button primary disabled text='Primary disabled'
  stack horizontal
    button compound text='Compound' secondaryText='This is a secondary text'
    button primary compound text='Primary compound' secondaryText='This is a secondary text'
  stack horizontal
    button icon='Add' text='Button with icon'
    button primary icon='Delete' text='Delete'
  stack horizontal
    button commandBar icon='Add' text='New item'
    button commandBar icon='Mail' text='Send'
  stack horizontal
    button icon='Emoji2'
    button icon='Calendar'
  stack horizontal
    button action icon='AddFriend' text='Create account'
    button action icon='Add' text='New item'
  stack horizontal
    button action icon='Globe' text='Pglet website' url='https://pglet.io' newWindow
    button icon='MyMoviesTV' text='Go to Disney' url='https://disney.com' newWindow
  stack horizontal
    button primary text='Button with menu'
      item text='New' icon='Add'
        item text='Email message' icon='Mail'
        item text='Calendar event' icon='Calendar'
      item text='Share' icon='Share'
        item text='Share to Twitter' key='sharetotwitter'
        item text='Share to Facebook' key='sharetofacebook'    
"

# while($true) {
#     Wait-PgletEvent $pageID
# }
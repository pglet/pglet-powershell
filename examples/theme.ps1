Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

Connect-PgletPage -Name "index" -NoWindow

Invoke-Pglet "clean page"

Invoke-Pglet "set page horizontalAlign='stretch' themePrimaryColor='#3ee66d' themeTextColor='#edd2b7' themeBackgroundColor='#262626'"

Invoke-Pglet "add
  textbox
  stack horizontal horizontalAlign=space-between
    text value='Theme selector example' size=large
    toggle id=theme label=Theme inline onText=Light offText=Dark
  stack
    text value='Code sample' size=xlarge
    text markdown value='# Heading 1\n## Heading 2\n### Heading 3\n#### Heading 4\n##### Heading 5\n\n``````powershell\nInvoke-Pglet `"clean page`"\n``````\n\nJust another [link](https://pglet.io)\n\nParagraph 1\n\nParagraph 2'
    text pre size=small value='`$i = 1\nWrite-Host `"Test 1`"'
    text pre size=medium value='`$i = 2\nWrite-Host `"Test 2`"'
    text pre size=large value='`$i = 3\nWrite-Host `"Test 3`"'
    link url='https://pglet.io' value='Go to Pglet'
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
    button toolbar icon='Add' text='New item'
    button toolbar icon='Mail' text='Send'
    button toolbar icon='ChevronDown' text='Show example'
    button toolbar icon='Delete' iconColor=orange20 text='Delete'
  stack horizontal
    button icon='Emoji2' title=Emoji!
    button icon='Calendar' title=Calendar!
  stack horizontal
    button ghost icon='AddFriend' text='Create account'
    button ghost icon='Add' text='New item'
  stack horizontal
    button action icon='Globe' text='Pglet website' url='https://pglet.io' newWindow
    button icon='MyMoviesTV' text='Go to Disney' url='https://disney.com' newWindow
  stack horizontal
    button text='Button with menu'
      item id=new text='New' icon='Add'
        item text='Email message' icon='Mail'
        item text='Calendar event' icon='Calendar'
      item id=share text='Share' icon='Share'
        item id=twitter text='Share to Twitter' data='sharetotwitter'
        item id=facebook text='Share to Facebook' data='sharetofacebook'
      item
        item data='key1'
    button primary split text='Primary with split'
      item text='New' icon='Add'
        item text='Email message' icon='Mail'
        item text='Calendar event' icon='Calendar'
      item text='Share' icon='Share' split
        item text='Share to Twitter' key='sharetotwitter'
        item text='Share to Facebook' key='sharetofacebook'
        item text='Share to Somewhere' disabled
        item text='Share to Email' key='sharetoemail'
          item text='Share to Outlook'
          item text='Share to Gmail'
      item divider
      item text='To to Google' icon='Globe' iconColor=green url='https://google.com' newWindow secondaryText='New window'
"

while($true) {
    $e = Wait-PgletEvent $pageID
    if ($e.target -eq 'theme') {
      if ($e.data -eq 'true') {
        # Light theme
        Invoke-Pglet "set page horizontalAlign='stretch' themePrimaryColor='' themeTextColor='' themeBackgroundColor=''"
      } else {
        # Dark theme
        Invoke-Pglet "set page horizontalAlign='stretch' themePrimaryColor='#3ee66d' themeTextColor='#edd2b7' themeBackgroundColor='#262626'"
      }
    }
}
Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

Connect-PgletPage -Name "index" -NoWindow

Invoke-Pglet "clean page"

Invoke-Pglet "add
text value=Squares size=large
stack horizontal
  text value='left top' align=left verticalAlign=top width=100 height=100 bgColor=Salmon color=white padding=5
  text value='center top' size=large align=center verticalAlign=top width=100 height=100 bgColor=Salmon color=white padding=5 borderColor='#555' borderStyle=solid borderWidth=1
  text value='right top' align=right verticalAlign=top width=100 height=100 bgColor=Salmon color=white padding=5 borderColor='#666' borderStyle=solid borderWidth=2
stack horizontal
  text value='left center' align=left verticalAlign=center width=100 height=100 bgColor=PaleGoldenrod padding=5
  text value='center center' size=large align=center verticalAlign=center width=100 height=100 bgColor=PaleGoldenrod padding=5 padding=5 borderColor='#555' borderStyle=solid borderWidth=1
  text value='right center' align=right verticalAlign=center width=100 height=100 bgColor=PaleGoldenrod padding=5 borderColor='#666' borderStyle=solid borderWidth=2
stack horizontal
  text value='left bottom' align=left verticalAlign=center width=6rem height=6rem bgColor=PaleGreen padding=5
  text value='center bottom' size=large align=center verticalAlign=center width=6rem height=6rem bgColor=PaleGreen padding=5 padding=5 borderColor='#555' borderStyle=solid borderWidth=1
  text value='right bottom' align=right verticalAlign=center width=6rem height=6rem bgColor=PaleGreen padding=5 borderColor='#666' borderStyle=solid borderWidth=2
text value=Circles size=large
stack horizontal
  text value='regular' align=center verticalAlign=center width=100px height=100px bgColor=Salmon color=white borderRadius=50
  text bold italic value='bold italic' align=center verticalAlign=center width=100 height=100 bgColor=PaleGoldenrod borderRadius=50 borderColor='#555' borderStyle=solid borderWidth=1
  text bold value='bold' align=center verticalAlign=center width=100 height=100 bgColor=PaleGreen color='#555' borderRadius=50 borderColor='#555' borderStyle=solid borderWidth=2
"
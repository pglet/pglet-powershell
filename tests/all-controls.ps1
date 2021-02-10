Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

Connect-PgletPage -Name "index" -NoWindow

Invoke-Pglet "clean page"

Invoke-Pglet "add
link url='http://google.com' value='Visit Google' newWindow
link value='Link without URL' size=large
link url='http://google.com' value='Cannot visit this link' disabled
"

Invoke-Pglet "add
stack horizontal
  icon name=ChangeEntitlements color=SlateGray
  icon name=Shop color=Khaki
  icon name=TrainSolid"

Invoke-Pglet "add
  stack horizontal verticalAlign=center
    icon name=BlockedSite color=Maroon size=25px
    icon name=Settings color=SlateBlue size=50px
    icon name=Save size=100px"

Invoke-Pglet "add text pre value='Line 1\nLine 2\nLine 3 ;lvj dfljs dklfjs lkfj lfkjsd flkjsd lfkjsd lkfjsdlkfjsl \nf;lvj dfljs dklfjs lkfj lfkjsd flkjsd lfkjsd lkfjsdlkfjsl f;lvj\n dfljs dklfjs lkfj lfkjsd flkjsd lfkjsd lkfjsdlkfjsl f;lvj dfljs dklfjs lkfj lfkjsd flkjsd lfkjsd lkfjsdlkfjsl f;lvj \ndfljs dklfjs lkfj lfkjsd flkjsd lfkjsd lkfjsdlkfjsl f;lvj dfljs dklfjs lkfj lfkjsd flkjsd lfkjsd lkfjsdlkfjsl f'"

# Standard button
Invoke-Pglet "add text value='Standard button' size='xLarge'"
$c = 'add button text="Standard"'
Invoke-Pglet "add text value='$c'"
Invoke-Pglet $c

# Primary button
Invoke-Pglet "add text value='Primary button' size='xLarge'"
$c = 'add button primary text="Primary"'
Invoke-Pglet "add text value='$c'"
Invoke-Pglet $c

# Dropdown
Invoke-Pglet "add dropdown value=A
option key=A text='Item A'
option key=B text='Item B'
"

# Unknown control
Invoke-Pglet "add some-control value='Line 1 Line 2 Line 3 ;lvj dfljs d' primary text='Primary'
"
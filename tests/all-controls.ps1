Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

Connect-PgletPage -Name "index" -NoWindow

Invoke-Pglet "clean page"

Invoke-Pglet "add text pre value='Line 1\nLine 2\nLine 3 ;lvj dfljs dklfjs lkfj lfkjsd flkjsd lfkjsd lkfjsdlkfjsl f;lvj dfljs dklfjs lkfj lfkjsd flkjsd lfkjsd lkfjsdlkfjsl f;lvj dfljs dklfjs lkfj lfkjsd flkjsd lfkjsd lkfjsdlkfjsl f;lvj dfljs dklfjs lkfj lfkjsd flkjsd lfkjsd lkfjsdlkfjsl f;lvj dfljs dklfjs lkfj lfkjsd flkjsd lfkjsd lkfjsdlkfjsl f;lvj dfljs dklfjs lkfj lfkjsd flkjsd lfkjsd lkfjsdlkfjsl f'"

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
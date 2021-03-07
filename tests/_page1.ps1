Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'src', 'Pglet.PowerShell', 'bin', 'Debug', 'netstandard2.0', 'Pglet.PowerShell.dll'))

$page2 = Connect-PgletPage "index2" -NoWindow

Invoke-Pglet "add
text value='aaabbb123sss'
button id=submit text='Submit form'
" -Page $page2
#$page.Connection.Send("add text value='aaa'")

while($true) {
    $e = Wait-PgletEvent
    Write-Host "$($e.target) $($e.name)"
}

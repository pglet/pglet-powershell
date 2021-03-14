Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

$page2 = Connect-PgletPage "index" -NoWindow

ipg "clean page"

Invoke-Pglet "add
text value='aaabbb123sss'
button id=submit text='Submit form'
" -Page $page2
#$page.Connection.Send("add text value='aaa'")

while($true) {
    $e = Wait-PgletEvent
    Write-Host "$($e.target) $($e.name)"
}

#Disconnect-Pglet
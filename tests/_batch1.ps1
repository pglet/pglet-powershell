Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

$page2 = Connect-PgletPage "index" -NoWindow

ipg "clean page"

Invoke-Pglet "add text value='Batch' size=xxLarge"

Invoke-Pglet "begin"
Invoke-Pglet "add text value='Line1'"
Invoke-Pglet "add text value='Line2'"
Invoke-Pglet "end"
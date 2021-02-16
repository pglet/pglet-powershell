Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

Connect-PgletPage -Name "index" -NoWindow

Invoke-Pglet "clean page"

Invoke-Pglet "add
text value='Item 1'
text value='Item 2'
text value='Item 3'
text value='Item 4'
text value='Item 5'
"

Invoke-Pglet "add trim=5
text value='Item 6'
"

Invoke-Pglet "add at=0 trim=-5
text value='Item 7'
"

for($i = 8; $i -lt 40; $i++) {
  Invoke-Pglet "add text trim=5 value='Item $i'"
  Start-Sleep -s 1
}
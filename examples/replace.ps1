Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

Connect-PgletPage -Name "index" -NoWindow

Invoke-Pglet "clean page"

Invoke-Pglet "add
  stack id=panel
    text value='Line 1'
    text value='Line 2'
    text value='Line 3'
"

Start-Sleep -s 2

Invoke-Pglet "replace to=panel at=0
text value='Replaced Line 1'"

Start-Sleep -s 2

Invoke-Pglet "replace to=panel at=1
text value='Replaced Line 2'"

Start-Sleep -s 2

Invoke-Pglet "replace to=panel at=2
text value='Replaced Line 3'"

Start-Sleep -s 2

Invoke-Pglet "remove panel at=2"

Start-Sleep -s 2

Invoke-Pglet "replace to=panel
text value='Replaced all lines!'"

Start-Sleep -s 2

Invoke-Pglet "clean panel"

# while($true) {
#     Wait-PgletEvent $pageID
# }
Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

Connect-PgletPage -Name "index" -NoWindow

Invoke-Pglet "clean page"

Invoke-Pglet "add
  stack id=panel
    text value='Hello, world!'"

Invoke-Pglet "replace to=panel
text value='Replaced text.'"

# while($true) {
#     Wait-PgletEvent $pageID
# }
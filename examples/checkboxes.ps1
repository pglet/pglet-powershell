Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

$page = Connect-PgletPage -Name "index" -NoWindow
Clear-PgletPage

$page.HorizontalAlign = 'start'
$page.update()

$stack = Stack -Controls @(
  Checkbox -Label 'Regular checkbox'
  Checkbox -Label 'Regular checkbox and checked' -Value $true
  Checkbox -Label 'Checkbox with tick on a right' -BoxSide 'end'
)

$page.add($stack)
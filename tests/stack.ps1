Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

$page = Connect-PgletPage -Name "index" -NoWindow
Clear-PgletPage

$stack = Stack -Horizontal -Wrap
$page.add($stack)

for($i = 0; $i -lt 20; $i++) {
  $stack.controls.add((Text -Value "$i" -Align Center -VerticalAlign Center -Width 100 -Height 100 -BgColor Salmon -Color White -Padding 5))
}

$stack.update()
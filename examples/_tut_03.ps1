Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

$page = Connect-PgletPage -Name "page-1" -NoWindow
$page.Clean()
$page.Add((Text -Value "Hello, world!"))
$page.Close()
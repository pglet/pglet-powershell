Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

$page1 = Connect-PgletPage -Name "page-1"
$page2 = Connect-PgletPage -Name "page-2"

$page1.Clean()
$page2.Clean()

$page1.Add((Text -Value "Hello, page 1!"))
$page2.Add((Text -Value "Hello, page 2!"))

$page1.Close()
$page2.Close()
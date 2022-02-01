Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

$page = Connect-PgletPage -Name "html-test"
$page.clean()

$h = Html '<h1>Hello, world!</h1><iframe src=''https://pglet.io''></iframe>'
$page.add($h)
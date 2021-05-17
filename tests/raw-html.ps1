Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

$page = Connect-PgletPage -Local -Name "index" -NoWindow
$page.clean()

$h = html '<div>hello!</div><iframe src=''https://google.com''></iframe>'
$page.add($h)

# while($true) {
#     Wait-PgletEvent $pageID
# }
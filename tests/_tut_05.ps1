Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

$page = Connect-PgletPage -Name "page-2" -NoWindow

$hostname = [Environment]::MachineName
$st = Stack -Id "st_$hostname" -Controls @(
    Text -Id MyText -Value "$hostname - $(Get-Date)"
)

$page.Add($st)
$page.Close()
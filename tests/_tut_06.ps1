Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

$page = Connect-PgletPage -Name "page-3" -NoWindow

$hostname = [Environment]::MachineName
$st = Stack -Id "st_$hostname"
$page.Add($st)
Write-Host $st.Uid
$st.Clean()

$st.Controls.Add((Text -Value "$hostname - $(Get-Date)"))
$page.Update()
$page.Close()
Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psm1'))

Connect-PgletPage -Name account1/page1

Write-Host "Last connection: $PGLET_CONNECTION_ID"

ipg "set page title='test'"

Disconnect-Pglet
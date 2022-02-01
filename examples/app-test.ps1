Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

Connect-PgletApp -Name 'index' -ScriptBlock {
    $hash = Invoke-Pglet "get page hash"
    
    Invoke-Pglet "add text value='Hello to connection $PGLET_CONNECTION_ID! with hash $hash'"
    Invoke-Pglet "add link url='#main'"
    Invoke-Pglet "add link url='#settings'"

    Start-Sleep -s 4
    $hash = Invoke-Pglet "set page hash=a1"

    Start-Sleep -s 4
    $hash = Invoke-Pglet "set page hash=a2"
}
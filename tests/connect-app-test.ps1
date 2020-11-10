Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent, 'pglet.psm1'))

Connect-PgletApp account1/page1 -Handler {
    param($h, $ConnectionID)
    $h.UI.WriteLine("Hello: $ConnectionID")
    Start-Sleep -s 3
}
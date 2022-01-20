Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

$page = Connect-PgletPage -Name "page-4" -NoWindow

try {
    $page.Clean()

    $btn = Button -Text "Click me!"
    $page.Add($btn)
    Wait-PgletEvent

} finally {
    $page.Close()
}

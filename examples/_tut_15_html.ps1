Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

$page = Connect-PgletPage -Name "text-1" -NoWindow

try {
    $page.Clean()

    $html = Html -Value '<h1>Hello, world!</h1>
    <p>This is a test paragraph with a <a href="https://pglet.io">link</a>.</p>'
    $page.Add($html)

} finally {
    $page.Close()
}
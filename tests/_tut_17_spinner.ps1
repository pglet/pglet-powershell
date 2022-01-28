Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

$page = Connect-PgletPage -Name "text-1" -NoWindow

try {
    $page.Clean()
    $page.Padding = 20

    $sp = Spinner -Label "Please wait while the operation is running..." -LabelPosition Right
    $page.Add($sp)

}
finally {
    $page.Close()
}
Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

$page = Connect-PgletPage -Name "text-1"

try {
    $page.Clean()

    $text = Text -Value 'Centered Text' -Size xlarge -Align Center -VerticalAlign Center -Width 100 -Height 100 `
      -Color 'White' -BgColor 'Salmon' -Padding 5 -Border '1px solid #555' -BorderRadius 10
    $page.Add($text)

} finally {
    $page.Close()
}
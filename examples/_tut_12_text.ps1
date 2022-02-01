Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

$page = Connect-PgletPage -Name "text-1" -NoWindow

try {
    $page.Clean()

    $text = Text -Value 'Centered Text' -Size xlarge -Align Center -VerticalAlign Center -Width 100 -Height 100 `
      -Color 'White' -BgColor 'Salmon' -Padding 5 -Border '1px solid #555'
    $page.Add($text)

    for($i = 0; $i -le 50; $i++) {
      $text.Value = "Radius $i"
      $text.BorderRadius = $i
      $page.Update()
      Start-Sleep -Milliseconds 50
    }

} finally {
    $page.Close()
}
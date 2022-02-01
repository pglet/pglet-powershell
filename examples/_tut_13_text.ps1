Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

$page = Connect-PgletPage -Name "text-1" -NoWindow

try {
    $page.Clean()

    for($i = 0; $i -le 20; $i++) {
      $page.Controls.Add((Text "Line $i"))
      if ($i -gt 4) {
        $page.Controls.RemoveAt(0)
      }
      $page.Update()
      Start-Sleep -Milliseconds 300
    }

} finally {
    $page.Close()
}
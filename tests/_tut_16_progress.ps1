Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

$page = Connect-PgletPage -Name "text-1" -NoWindow

try {
    $page.Clean()

    # copy file
    $prog1 = Progress -Label "Copying /file1.txt to /file2.txt" -Width "30%" -BarHeight 4
    $page.Add($prog1)
  
    for($i = 0; $i -le 100; $i=$i+5) {
      $prog1.Value = $i
      $prog1.Update()
      Start-Sleep -Milliseconds 100
    }

} finally {
    $page.Close()
}
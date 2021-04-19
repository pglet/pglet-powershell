Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

Connect-PgletPage -Name "index" -NoWindow

Invoke-Pglet "clean page"

Invoke-Pglet "
add
  tabs id=tabs1
    tab text=Home
    tab text=Products
    tab text=Services
"

Start-Sleep -s 3
Invoke-Pglet "set tabs1 value=Products"

Start-Sleep -s 2
Invoke-Pglet "set tabs1 value=Services"

Start-Sleep -s 2
Invoke-Pglet "set tabs1 value=''"
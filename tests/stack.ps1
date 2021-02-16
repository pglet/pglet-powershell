Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

Connect-PgletPage -Name "index" -NoWindow

Invoke-Pglet "clean page"

Invoke-Pglet "add
stack id=items horizontal wrap
  text value='1' align=center verticalAlign=center width=100 height=100 bgColor=Salmon color=white padding=5
"

for($i = 0; $i -lt 20; $i++) {
  Invoke-Pglet "add text to=items value='$i' align=center verticalAlign=center width=100 height=100 bgColor=Salmon color=white padding=5"
}
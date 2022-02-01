Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

Connect-PgletPage -Name "index" -NoWindow

Invoke-Pglet "clean page"
Invoke-Pglet "set page padding=10 gap=10 horizontalAlign='start'"

Invoke-Pglet "add
panel autodismiss=false id=panel title='Missing Subject' lightDismiss=false type=custom width=800 blocking=true
  choicegroup
    option key=red
    option key=green
    option key=blue
  footer
    stack horizontal
      button id=yes primary text=Yes
      button id=no text=No
button id=open text='Open panel'
choicegroup
  option key=1
  option key=2
  option key=3
"

while($true) {
  $e = Wait-PgletEvent $pageID
  if ($e.target -eq 'open') {
    Invoke-Pglet "set panel open=true"
  } elseif ($e.target -eq 'panel' -and $e.name -eq 'dismiss') {
    Invoke-Pglet "set panel open=false"
  } elseif ($e.target -eq 'panel:yes' -or $e.target -eq 'panel:no') {
    Invoke-Pglet "set panel open=false"
  }
}
Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

Connect-PgletPage -Name "index" -NoWindow -Ticker 2000

Invoke-Pglet "clean page"
Invoke-Pglet "set page padding=10 gap=10 horizontalAlign='start'"

Invoke-Pglet "add
button id=openBasic text='Open basic dialog'
dialog id=dialogBasic blocking=false type=largeHeader title='Missing Subject' subText='Do you want to send this message without a subject?'
  footer
    button id=yes primary text=Yes
    button id=no text=No

button id=open text='Open dialog'
dialog id=dialog blocking=true type=close title='Missing Subject' subText='Do you want to send this message without a subject?' width=600
  choicegroup
    option key=red
    option key=green
    option key=blue
  footer
    button id=yes primary text=Yes
    button id=no text=No
"

<#
  button id=openBasic text='Open basic dialog'
  dialog id=dialogBasic blocking=false largeHeader=false close title='Missing Subject' subText='Do you want to send this message without a subject?' hidden
    footer
      button id=yes primary text=Yes
      button id=no text=No
#>

while($true) {
  $e = Wait-PgletEvent $pageID
  if ($e.target -eq 'openBasic') {
    Invoke-Pglet "set dialogBasic open=true"
  } elseif ($e.target -eq 'open') {
    Invoke-Pglet "set dialog open=true"
  } elseif ($e.target -eq 'dialog:yes' -or $e.target -eq 'dialog:no') {
    Invoke-Pglet "set dialog open=false"
  } else {
    $e
  }
}
Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

Connect-PgletPage -Name "index" -NoWindow

Invoke-Pglet "clean page"
Invoke-Pglet "set page horizontalAlign='start'"

Invoke-Pglet "add
  dropdown id=colors placeholder='Select color' value='green'
    option key=red
    option key=green
    option key=blue
  dropdown id=cal1 placeholder='Pick one icon'
    option key=day text=Day icon=CalendarDay iconColor=red
    option key=week text=Week icon=CalendarWeek
    option key=month text=Month icon=Calendar
"

Start-Sleep -s 3
Invoke-Pglet "set colors value=blue"

Start-Sleep -s 3
Invoke-Pglet "set cal1 value=week"

Start-Sleep -s 3
Invoke-Pglet "set cal1 value=''"
Invoke-Pglet "set colors value=''"

# while($true) {
#     Wait-PgletEvent $pageID
# }
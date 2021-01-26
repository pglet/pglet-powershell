Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

Connect-PgletPage -Name "index" -NoWindow

Invoke-Pglet "clean page"
Invoke-Pglet "set page horizontalAlign='start'"

Invoke-Pglet "add
  link url='http://google.com' value='Visit Google' newWindow
  link url='http://google.com' value='Cannot visit this link' disabled
  choicegroup label='Select color' value='green'
    option key=red
    option key=green
    option key=blue
  choicegroup label='Pick one icon'
    option key=day text=Day icon=CalendarDay
    option key=week text=Week icon=CalendarWeek
    option key=month text=Month icon=Calendar
"

# while($true) {
#     Wait-PgletEvent $pageID
# }
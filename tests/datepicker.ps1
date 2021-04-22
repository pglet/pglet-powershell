Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

Connect-PgletPage -Name "pglet-datepicker"

Invoke-Pglet "clean page"
Invoke-Pglet "set page horizontalAlign='start'"

$d = Get-Date

Invoke-Pglet "add
  DatePicker label='Start date' value='$d'
  DatePicker label='End date'
  DatePicker label='Allow text input' allowTextInput
  DatePicker label='Allow text input with placeholder' placeholder='Select date...' allowTextInput  width='50%' height='300px'
  DatePicker value='01/01/2012' label='Required' placeholder='Select date...' required allowTextInput width='50%' align=right
  DatePicker label='Wrong date should set to empty' value='15/15/2001'
"
Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

Connect-PgletPage -Name "index" -NoWindow

Invoke-Pglet "clean page"
Invoke-Pglet "set page horizontalAlign='start'"

Invoke-Pglet "add
  stack
    toggle label='Enabled and checked' value='true'
    toggle label='Enabled and unchecked'
    toggle disabled label='Disabled and checked' value='true'
    toggle disabled label='Disabled and unchecked'
    toggle inline label='With inline label' onText=On offText=Off
    toggle disabled inline label='Disabled with inline label' onText=On offText=Off
    toggle inline label='With inline label and without onText and offText'
    toggle disabled inline label='Disabled with inline label and without onText and offText'  
"

# while($true) {
#     Wait-PgletEvent $pageID
# }
Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

Connect-PgletPage -Name "index" -NoWindow

Invoke-Pglet "clean page"
Invoke-Pglet "set page horizontalAlign='start'"

Invoke-Pglet "add
  stack
    checkbox label='Regular checkbox'
    checkbox label='Regular checkbox and checked' value='true'
    checkbox label='Checkbox with tick on a right' boxSide='end'
"

# while($true) {
#     Wait-PgletEvent $pageID
# }
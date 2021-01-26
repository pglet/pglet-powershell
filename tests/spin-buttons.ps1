Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

Connect-PgletPage -Name "index" -NoWindow

Invoke-Pglet "clean page"
Invoke-Pglet "set page horizontalAlign='start'"

Invoke-Pglet "add
  stack width='50%'
    spinButton label='Spin button with ranges' min='-5' max=15 step=1 value=-2
"

#

# while($true) {
#     Wait-PgletEvent $pageID
# }
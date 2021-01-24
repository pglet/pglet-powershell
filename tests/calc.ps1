Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

Connect-PgletPage -Name "index" -NoWindow

Invoke-Pglet "clean page"

Invoke-Pglet "add
  textbox
  stack horizontal
    textbox
  stack horizontal
    button text='7'    
    button text='8'
    button text='9'
    button text='/'
  stack horizontal
    button text='4'    
    button text='5'
    button text='6'
    button text='*'
"

# while($true) {
#     Wait-PgletEvent $pageID
# }
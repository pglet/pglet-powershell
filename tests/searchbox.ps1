Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

Connect-PgletPage -Name "index" -NoWindow

Invoke-Pglet "clean page"

Invoke-Pglet "add
  searchbox underlined placeholder='Search files and folders' icon=Filter iconColor=red width='100%'
  searchbox disabled placeholder='Search something...'
"

# while($true) {
#     Wait-PgletEvent $pageID
# }
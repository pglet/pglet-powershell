Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

Connect-PgletPage -Name "index" -NoWindow

Invoke-Pglet "clean page"
Invoke-Pglet "set page horizontalAlign='stretch'"

Invoke-Pglet "add
  grid compact=false selection=multiple preserveSelection headerVisible=true
    columns
      column onClick name='File Type' icon=Page iconOnly fieldName='iconName' minWidth=60 maxWidth=200
      column resizable sortable name='Name' fieldName='name'
      column sortable=number name='Action' fieldName='key' minWidth=100
        link url='http://{key}'     
    items id=gridItems
      item key=1 name='Item 1' iconName='ItemIcon1'
      item key=2 name='Item 2' iconName='ItemIcon2'
      item key=3 name='Item 3' iconName='ItemIcon3'
"

Start-Sleep -s 5

Invoke-Pglet "clean gridItems"
$cmd = "addf to=gridItems`n"
for ($i = 0; $i -lt 500; $i++) {
  #Invoke-Pglet "clear gridItems at=0"
  $cmd = "$($cmd)item to=gridItems key=$i name='Item $i' iconName='ItemIcon$i'`n"
  #Start-Sleep -ms 20
}
#Write-Host $lines
Invoke-Pglet $cmd

# while($true) {
#     Wait-PgletEvent $pageID
# }
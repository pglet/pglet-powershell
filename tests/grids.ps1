Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

Connect-PgletPage -Name "index" -NoWindow

Invoke-Pglet "clean page"
Invoke-Pglet "set page padding=0 horizontalAlign=''"

Invoke-Pglet "add
stack horizontal horizontalAlign='stretch'
  stack minwidth='250px'
    nav
      item text='Group 1'
        item text='New'
          item key='email' text='Email message' icon='Mail'
          item key='calendar' text='Calendar event' icon='Calendar'
      item text='Group 2'
        item key=share text='Share' icon='Share'
        item key=twitter text='Share to Twitter'
  stack width='100%' horizontal horizontalAlign='stretch'
    stack width='70%'
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
    stack width='30%'
      text value=111
"

#Start-Sleep -s 5

#Invoke-Pglet "clean gridItems"
#$cmd = "addf to=gridItems`n"
for ($i = 0; $i -lt 100; $i++) {
  #Invoke-Pglet "clear gridItems at=0"
  Invoke-Pglet "add item to=gridItems key=item$i name='Item $i' iconName='ItemIcon$i'`n"
  Start-Sleep -ms 10
}
#Write-Host $lines
Invoke-Pglet $cmd

# while($true) {
#     Wait-PgletEvent $pageID
# }
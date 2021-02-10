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
    grid compact=false selection=none preserveSelection headerVisible=true
      columns
        column onClick name='File Type' icon=Page iconOnly fieldName='iconName' minWidth=20 maxWidth=20
          icon name=FileTemplate size=16
        column resizable sortable name='Name' fieldName='name'
          text value='{name}'
        column resizable name='Write' fieldName='write'
          stack horizontal height='100%' verticalAlign=center
            checkbox value='{write}'
        column resizable name='Color' fieldName='read'
          dropdown value='{color}' data='{key}'
            option key=red text='Red'
            option key=green text='Green'
            option key=blue text='Blue'
        column resizable sortable name='Description' fieldName='description'
          textbox value='{description}'
        column sortable=number name='Action' fieldName='key' minWidth=150
          stack horizontal height='100%' verticalAlign=center
            link url='{key}' value='{iconName}' visible=false
            link url='{key}' value='{name}' visible=false
            button icon='Edit' title='Edit todo' width=16 height=16 visible=true data='{key}'
            button icon='Delete' iconColor=red title='Delete todo' width=16 height=16 visible=true data='{key}'
      items id=gridItems
        item key=1 name='Item 1' iconName='ItemIcon1' description='Descr A'
        item key=2 name='Item 2' iconName='ItemIcon2' description='Descr B'
        item key=3 name='Item 3' iconName='ItemIcon3' description='Descr C'
"

#Start-Sleep -s 5

#Invoke-Pglet "clean gridItems"
#$cmd = "addf to=gridItems`n"
for ($i = 0; $i -lt 5; $i++) {
  #Invoke-Pglet "clear gridItems at=0"
  Invoke-Pglet "add item to=gridItems key=item$i name='Item $i' iconName='ItemIcon$i'`n"
  Start-Sleep -s 2
}
#Write-Host $lines
#Invoke-Pglet $cmd

# while($true) {
#     Wait-PgletEvent $pageID
# }
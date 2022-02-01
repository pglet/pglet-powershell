Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

Connect-PgletPage -Name "index" -Local -NoWindow

Invoke-Pglet "clean page"
Invoke-Pglet "set page horizontalAlign='center' ThemePrimaryColor = '#e180ff' ThemeTextColor = '#e1e4e8' ThemeBackgroundColor = '#24292e'"

Invoke-Pglet "add
stack width='50%'
  toolbar
    item text='New' icon='Add'
      item text='Todo item' icon='TaskLogo'
    far
      item text=Info icon=Info iconOnly
  stack horizontal horizontalAlign='space-between'
    textbox onChange width='100%'
    stack horizontal gap=0
      button icon='Edit' title='Edit todo'
      button icon='Delete' iconColor=red title='Delete todo'
  stack gap=40
    tabs
      tab key=all
      tab key=active
      tab key=completed
    stack horizontal horizontalAlign='space-between'
      text value='0 items left'
      button text='Clear completed'
"

# while($true) {
#     Wait-PgletEvent $pageID
# }
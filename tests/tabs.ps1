Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

Connect-PgletPage -Name "index" -NoWindow

Invoke-Pglet "clean page"

Invoke-Pglet "
add tabs solid id=t1
  tab key='' text='Tab 1' icon=Globe
    stack horizontal
      text value='This is tab1'
      text value='This is tab1 - line2'
  tab id=tab2 key=20 text='Tab 2'
    stack gap=10
      text value='This is tab2'
      text value='This is tab2 - line2'
  tab text='Tab 3' icon=Ringer
    stack gap=10
      text value='This is tab3'
      text value='This is tab3 - line2'
  tab
    button
  tab key='tab33'
"

Start-Sleep -s 3

Invoke-Pglet "set t1 value=30"
Invoke-Pglet "set t1:tab2 visible=false"

Invoke-Pglet "add to=t1
tab key=40 text='Tab 4'
  stack
    text value='This is tab4'
    text value='This is tab4 - line2'
    tabs margin=10px
      tab text=JavaScript icon=ChevronDown count=10
        textbox label='First name'
      tab text='C#' count=30
        button text='Hello!'
      tab text=Python count=0
        text value='PPPP!'"

# while($true) {
#     Wait-PgletEvent $pageID
# }
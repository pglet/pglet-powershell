Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

Connect-PgletPage -Name "index" -NoWindow

Invoke-Pglet "clean page"

Invoke-Pglet "add
text value='Pie chart' size=xlarge
pieChart legend tooltips width='100%' innerRadius=40 innerValue=42
  data
    p legend='Free space' value=20 tooltip='20%'
    p legend='Total space' value=50 tooltip='50%'
    p legend='Reserved space' value=30 tooltip='30%'
    p legend='A' value=1 tooltip='20%'
    p legend='B' value=2 tooltip='50%'
    p legend='C' value=3 tooltip='30%'    
"
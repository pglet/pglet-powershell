Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

Connect-PgletPage -Name "index" -NoWindow

Invoke-Pglet "clean page"
Invoke-Pglet "set page"

Invoke-Pglet "add
text value='Vertical chart with textual x axis' size=xlarge
button primary text=Test!
verticalBarChart tooltips legend=false width='50%' height=300 yticks=5 barWidth=20 yFormat='{y}%'
  data
    p id=rdp x=Red y=20 color=Crimson legend=Red ytooltip='20%'
    p x=Green y=50 color=ForestGreen legend=Green ytooltip='50%'
    p x=Blue y=30 color=blue legend=Blue ytooltip='30%'

text value='Vertical chart with numeric x axis' size=xlarge
verticalBarChart tooltips legend=false width='100%' xtype=string height=400 ymax=100 yticks=5 barWidth=20 yFormat='`${y}'
  data id=d1
"

for($i = 20; $i -lt 41; $i++) {
  Invoke-Pglet "setf rdp y=$i"
  Start-Sleep -ms 100
}

# for($i = 0; $i -lt 51; $i++) {
# Invoke-Pglet "addf to=d1 p x=$i y=$($i*2)"
# Start-Sleep -ms 50
# }

for($i = 0; $i -lt 200; $i++) {
Invoke-Pglet "addf to=d1 at=0 trim=-50 p x=$i y=$(Get-Random -Maximum 100)"
Start-Sleep -ms 100
}

# $(Get-Random -Maximum 100)

# Start-Sleep -s 2
# Invoke-Pglet "add to=d1
#   p x=6 y=20"
# Invoke-Pglet "remove d1 at=0"

# Start-Sleep -s 2
# Invoke-Pglet "add to=d1
#   p x=7 y=10"
# Invoke-Pglet "remove d1 at=0"
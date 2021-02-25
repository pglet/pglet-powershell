Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

Connect-PgletPage -Name "index" -NoWindow

Invoke-Pglet "clean page"

Invoke-Pglet "add
text value='Line chart' size=xlarge
lineChart legend tooltips xtype=date yticks=10 ymax=100 yformat='{y}%' width='100%' height='50%'
  data legend='Line A'
    p x='2020-03-03T00:00:00.000Z' y=10
    p x='2020-03-03T10:00:00.000Z' y=50
    p x='2020-03-03T11:00:00.000Z' y=70
  data legend='Line B'
    p x='2020-03-03T00:00:00.000Z' y=30
    p x='2020-03-04T00:00:00.000Z' y=20
    p x='2020-03-05T00:00:00.000Z' y=90

text value='CPU load for the last hour' size=xlarge
lineChart legend tooltips xtype=date yticks=5 ymax=100 yformat='{y}%' width='50%' height='30%'
  data id=d1 legend='CPU'
  data id=d2 legend='Core 1'
  data id=d3 legend='Core 2'

text value='CPU temperature' size=xlarge
lineChart legend tooltips xtype=date yticks=4 yformat='{y} °C' width='50%' height='30%'
  data id=t1 legend='t, °C'
"

# Temperature
$points=@()
for($i = -60; $i -lt 0; $i++) {
  $d=(Get-Date).AddMinutes($i)
  $d = Get-Date -Year $d.Year -Month $d.Month -Day $d.Day -Hour $d.Hour -Minute $d.Minute -Second 0
  $t = $(Get-Random -Minimum -40 -Maximum 40)
  $points += "p x='$($d.tostring())' y='$t'"
}
Invoke-Pglet "addf to=t1 `n$($points -join "`n")"

# CPU
$points=@()
for($i = -60; $i -lt 0; $i++) {
  $d=(Get-Date).AddSeconds($i)
  $points += "p x='$d' y=0"
}
Invoke-Pglet "addf to=d1 `n$($points -join "`n")"

for($i = 0; $i -lt 200; $i++) {
  $d=(Get-Date)
  Write-Host "$i - $d"
  $tick=''
  if ($d.second % 15 -eq 0) {
    $tick=$d
  }
  $cpuLoad = $(Get-Random -Maximum 100)
  $coreLoad1 = [Math]::Abs($cpuLoad - $(Get-Random -Maximum 20))
  $coreLoad2 = [Math]::Abs($cpuLoad - $(Get-Random -Maximum 20))
  Invoke-Pglet "addf to=d1 trim=60 p x='$d' y=$cpuLoad tick='$tick'"
  Invoke-Pglet "addf to=d2 trim=60 p x='$d' y=$coreLoad1"
  Invoke-Pglet "addf to=d3 trim=60 p x='$d' y=$coreLoad2"
  Start-Sleep -s 1
}
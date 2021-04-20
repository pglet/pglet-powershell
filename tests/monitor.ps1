Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

function getTopProcesses($maxCount) {
  $procIDs = @{}
  (Get-Counter "\Process(*)\ID Process" -ErrorAction SilentlyContinue).CounterSamples |
    ForEach-Object { $procIDs[($_.Path -replace "\\id process$","\% Processor Time")] = $_.CookedValue }

  $CpuCores = (Get-CimInstance -ClassName Win32_ComputerSystem).NumberOfLogicalProcessors
  return (Get-Counter "\Process(*)\% Processor Time" -ErrorAction SilentlyContinue).CounterSamples |
    Where-Object {$_.InstanceName -notmatch "^(idle|_total|system)$"} |
    Sort-Object "CookedValue" -descending |
    Select-Object -first $maxCount `
        @{Name="PID";Expression={$procIDs[$_.Path]}},
        @{Name="Path";Expression={(Get-Process -id $procIDs[$_.Path]).Path}},
        InstanceName,
        @{Name="CPU";Expression={[Decimal]::Round(($_.CookedValue / $CpuCores), 2)}}
}

$userName = $env:UserName
$compName = $env:ComputerName
$totalRam = (Get-CimInstance Win32_PhysicalMemory | Measure-Object -Property capacity -Sum).Sum / 1024 / 1024

$page = Connect-PgletPage -Name "ps-monitor" -NoWindow
$page.title = 'Task Manager'
$page.padding = '10px'
$page.update()

$tab = Tab -Id $compName -Text $compName

$page.add(
    (Text -Id 'title' -Value 'Task Manager' -Size xLarge),
    (Tabs -Id 'computers' -Width '100%' -TabItems @(
    $tab
)))

$tab.clean()

$proc_grid = Grid -ShimmerLines 5 -SelectionMode Single -HeaderVisible -Columns @(
  GridColumn -Resizable -Sortable 'string' -FieldName 'name' -Name 'Process name' -MaxWidth 100
  GridColumn -Resizable -Sortable 'number' -FieldName 'pid' -Name 'PID' -MaxWidth 100
  GridColumn -Resizable -Sortable 'string' -FieldName 'cpu_display' -SortField 'cpu' -Name 'CPU %' -MaxWidth 100
  GridColumn -Resizable -Sortable 'string' -FieldName 'path' -Name 'Path'
)

# Generate 30 empty values for the last minute to initially fill charts
$points=@()
for($i = -30; $i -lt 0; $i++) {
  $d=(Get-Date).AddSeconds($i*2)
  $points += LineChartDataPoint -X $d -Y 0
}

$cpu_chart = New-PgletLineChartData -Legend 'CPU' -Points $points
$ram_chart = New-PgletLineChartData -Legend 'RAM' -Points $points

$stack = Stack -Horizontal -Controls @(
    Stack -Width '50%' -Controls @(
        $proc_grid
    )
    Stack -Width '40%' -Controls @(
        Text -Value 'CPU, %' -Size Large
        LineChart -Tooltips -XType Date -YTicks 5 -YMax 100 -YFormat '{y}%' -Height 250 -Lines @(
            $cpu_chart
        )
        Text -Value 'RAM, MB' -Size Large
        LineChart -Tooltips -XType Date -YTicks 5 -YMax $totalRam -Height 250 -Lines @(
            $ram_chart
        )
    )
)

$tab.controls.add($stack)
$tab.update()

# Main update loop
while($true) {

  $items = getTopProcesses 10 | ForEach-Object {
    [PSCustomObject]@{
      pid = $_.PID
      name = $_.InstanceName
      cpu = $_.CPU
      cpu_display = "$($_.CPU)%"
      path = $_.Path
    }
  }

  $proc_grid.items = $items
  $proc_grid.update()

  return


  # update top processes
  $cmd = "replace to=$($tabId):gridItems`n"
  $cmd += (getTopProcesses 10 |
    ForEach-Object { "item id=$($_.PID) pid=$($_.PID) name='$($_.InstanceName)' cpu=$($_.CPU) cpu_display='$($_.CPU)%' path='$($_.Path -replace '\\', '\\')'" }) -join "`n"
  Invoke-Pglet $cmd | Out-Null

  $d=(Get-Date)

  # Update CPU load
  $cpuLoad = (Get-Counter '\Processor(_Total)\% Processor Time').CounterSamples.CookedValue.ToString("#,0.00")
  Invoke-Pglet "addf to=$cpuChartId trim=30 p x='$d' y=$cpuLoad"

  # Update RAM load
  $availRam = (Get-Counter '\Memory\Available MBytes').CounterSamples.CookedValue
  $usedRam = $totalRam - $availRam
  $usedRamGB = ($usedRam/1024).ToString("#,0.00")
  Invoke-Pglet "addf to=$ramChartId trim=30 p x='$d' y=$usedRam ytooltip='$usedRamGB GB'"

  Start-Sleep -s 2
}

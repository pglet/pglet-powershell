Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

Connect-PgletPage -Name "index" -NoWindow

#Invoke-Pglet "clean page"
Invoke-Pglet "set page title='Top 10 CPU processes'"

$compName = $env:computername

Invoke-Pglet "add
text id=title value='Top 10 CPU processes' size=xLarge
tabs id=computers value=$compName width='100%'
  tab id=$compName text=$compName
"

$tabId = "computers:$compName"

Invoke-Pglet "clean $tabId"

Invoke-Pglet "add to=$tabId
grid compact=false selection=single preserveSelection=false headerVisible=true
  columns
    column resizable sortable fieldName='name' name='Process name' maxWidth=100
    column resizable sortable fieldName='pid' name='PID' maxWidth=100
    column resizable sortable sorted=desc fieldName='cpu_display' name='CPU %' maxWidth=100
    column resizable sortable fieldName='path' name='Path'
  items id=gridItems
    item pid=123 name='process 1' cpu=30 cpu_display='30%' path='C:\\program files\\process1.exe'
    item pid=644 name='process 2' cpu=20 cpu_display='20%' path='C:\\program files\\process2.exe'
"

#Start-Sleep -s 5

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

while($true) {
  $cmd = "add to=$($tabId):gridItems`n"
  $cmd += (getTopProcesses 10 |
    ForEach-Object { "item pid=$($_.PID) name='$($_.InstanceName)' cpu=$($_.CPU) cpu_display='$($_.CPU)%' path='$($_.Path -replace '\\', '\\')'" }) -join "`n"

  Invoke-Pglet "clean $($tabId):gridItems" | Out-Null
  Invoke-Pglet $cmd | Out-Null
  Start-Sleep -s 2
}
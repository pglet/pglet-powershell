Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

Connect-PgletPage -Name "index" -NoWindow

Invoke-Pglet "clean page"
Invoke-Pglet "set page title='Top 10 CPU processes' padding=10px"

$compName = $env:computername

Invoke-Pglet "add
text id=title value='Top 10 CPU processes' size=xLarge
tabs id=computers width='100%'
  tab id=$compName text=$compName
"

$tabId = "computers:$compName"

Invoke-Pglet "clean $tabId"

Invoke-Pglet "add to=$tabId
grid compact=false selection=single preserveSelection=true headerVisible=true
  columns
    column resizable sortable fieldName='name' name='Process name' maxWidth=100
    column resizable sortable fieldName='pid' name='PID' maxWidth=100
    column resizable sortable sorted=desc fieldName='cpu_display' name='CPU %' maxWidth=100
    column resizable sortable fieldName='path' name='Path'
  items id=gridItems
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
    ForEach-Object { "item id=$($_.PID) pid=$($_.PID) name='$($_.InstanceName)' cpu=$($_.CPU) cpu_display='$($_.CPU)%' path='$($_.Path -replace '\\', '\\')'" }) -join "`n"

  Invoke-Pglet "clean $($tabId):gridItems" | Out-Null
  Invoke-Pglet $cmd | Out-Null
  Start-Sleep -s 2
  #Invoke-Pglet "clean $($tabId):gridItems" | Out-Null
  #return
}
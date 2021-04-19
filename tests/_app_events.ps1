$testobj1 = New-Object -TypeName System.IO.FileSystemWatcher
$testobj1.Path = $env:USERPROFILE
Register-ObjectEvent -InputObject $testobj1 -EventName Created -Action {
    $file = $Event.SourceEventArgs.FullPath
    Write-Host "start $file";
    Start-Sleep -s 5;
    Write-Host "end $file"
}

$testobj2 = New-Object -TypeName System.IO.FileSystemWatcher
$testobj2.Path = "C:\projects\2"
Register-ObjectEvent -InputObject $testobj2 -EventName Created -Action {
    $file = $Event.SourceEventArgs.FullPath
    Write-Host "start $file";
    Start-Sleep -s 5;
    Write-Host "end $file"
}

while($true) { Start-Sleep -s 5 }
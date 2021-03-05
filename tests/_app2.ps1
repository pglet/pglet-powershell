$observableCollection = New-Object System.Collections.ObjectModel.ObservableCollection[object]

#Set up an event watcher
Register-ObjectEvent -InputObject $observableCollection -EventName CollectionChanged -Action {
    $Global:test = $Event
    Switch ($test.SourceEventArgs.Action) {
        "Add" {
            $test.SourceEventArgs.NewItems | ForEach {
                Write-Host ("{0} was added" -f $_) -ForegroundColor Yellow -BackgroundColor Black
            }
        }
        "Remove" {
            $test.SourceEventArgs.OldItems | ForEach {
                Write-Host ("{0} was removed" -f $_) -ForegroundColor Yellow -BackgroundColor Black
            }
        }
        Default {
            Write-Host ("The following action occurred: {0}" -f $test.SourceEventArgs.Action) -ForegroundColor Yellow -BackgroundColor Black
        }
    }
}
$observableCollection.Add(5) 
$observableCollection.Remove(5) 
$observableCollection.Clear()
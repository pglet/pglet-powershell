Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'src', 'Pglet.PowerShell', 'bin', 'Debug', 'netstandard2.0', 'Pglet.PowerShell.dll'))

New-PgletButton -Id "button1" -OnClick {
    Write-Host "This is button!"
}

Connect-PgletApp "index1" -NoWindow -ScriptBlock {
    [System.Console]::WriteLine("Hello!")
    Start-Sleep -s 10
    Write-Host "Bye!"
}
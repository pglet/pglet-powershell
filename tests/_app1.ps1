Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

New-PgletButton -Id "button1" -OnClick {
    Write-Host "This is button!"
}

Connect-PgletApp "index" -NoWindow -ScriptBlock {
    try {
        [System.Console]::WriteLine($PGLET_PAGE)
        [System.Console]::WriteLine((Get-Module 'pglet'))
        Invoke-Pglet "add text value='Hi, $($PGLET_PAGE.connection.pipeId)!'"
        Start-Sleep -s 60
        [System.Console]::WriteLine("Bye!")
    }
    finally {
        [System.Console]::WriteLine("Terminate, $($PGLET_PAGE.connection.pipeId)!")
    }
}
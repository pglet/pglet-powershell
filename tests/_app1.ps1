Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

Button -Id "button1" -OnClick {
    Write-Host "This is button!"
}

Connect-PgletApp "index" -NoWindow -ScriptBlock {
    try {
        Write-Trace $PGLET_PAGE
        Invoke-Pglet "add text value='Hi, $($PGLET_PAGE.connection.pipeId)!'"
        Start-Sleep -s 30
        Write-Trace "Bye!"
    }
    finally {
        Write-Trace "Terminate, $($PGLET_PAGE.connection.pipeId)!"
    }
}
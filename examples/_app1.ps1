$listener = new-object "system.diagnostics.consoletracelistener"
[System.Diagnostics.Trace]::Listeners.Add($listener) | Out-Null 

Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

Connect-PgletApp "ps-app-test" -ScriptBlock {
    try {
        $btn = Button -Text "Click me!" -OnClick {
            Write-Trace "Oh, I've been clicked!"
        }

        Write-Trace $PGLET_PAGE
        $page = $PGLET_PAGE
        $page.OnResize = {
            Write-Trace "New page size: $($page.winWidth), $($page.winHeight)"
        }
        $greeting = Text -Value "Hello, $($PGLET_PAGE.sessionId)"
        $page.add(@($greeting, $btn))
        #Start-Sleep -s 10
        Write-Trace "Bye!"
    }
    finally {
        Write-Trace "Terminate, $($PGLET_PAGE.sessionId)!"
    }
}
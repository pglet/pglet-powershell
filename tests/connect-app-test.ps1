Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psm1'))

Connect-PgletApp -Name account1/page1 -Public -ScriptBlock {
    #param($ui)

    function Method1() {
        Start-Sleep -s 5
        #Write-Trace("sid: " + $SESSION_ID)
        Send-Pglet "set page title='aaaabbb'"
    }    

    try {
        Write-Trace "before sleep: $PGLET_CONNECTION_ID"
        Method1
        Write-Trace "sid: $PGLET_CONNECTION_ID"
    } catch {
        Write-Trace "error: $_"
    }
}
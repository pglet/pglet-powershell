Remove-Module pglet -ErrorAction SilentlyContinue

Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psm1'))

function TestMethod() {
    Start-Sleep -s 5
    #Write-Trace("sid: " + $SESSION_ID)
}

Connect-PgletApp -Name account1/page1 -Public -ScriptBlock {
    #param($ui)

    function Method1() {
        Start-Sleep -s 5
        #Write-Trace("sid: " + $SESSION_ID)
        Write-Pglet "set page title='aaaabbb'"
    }    

    try {
        Write-Trace "before sleep: $SESSION_ID"
        Method1
        Write-Trace "sid: $SESSION_ID"
    } catch {
        Write-Trace "error: $_"
    }
}
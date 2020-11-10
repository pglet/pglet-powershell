Remove-Module pglet -ErrorAction SilentlyContinue

Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psm1'))

function TestMethod() {
    Start-Sleep -s 5
    #Write-Log("sid: " + $SESSION_ID)
}

Connect-PgletApp account1/page1 -Handler {
    #param($ui)

    function Method1() {
        Start-Sleep -s 5
        #Write-Log("sid: " + $SESSION_ID)
        Write-Pglet "aaaabbb"
    }    

    try {
        Write-Log("before sleep: " + $SESSION_ID)
        Method1
        Write-Log("sid: " + $SESSION_ID)
    } catch {
        Write-Log("error!" + $_.ToString())
    }
}
Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent, 'pglet.psm1'))

function TestMethod() {
    Start-Sleep -s 5
    #$ui.WriteLine("sid: " + $SESSION_ID)
}

Connect-PgletApp account1/page1 -Handler {
    #param($ui)

    function Method1() {
        Start-Sleep -s 5
        #$ui.WriteLine("sid: " + $SESSION_ID)
        Write-Pglet "aaaabbb"
    }    

    try {
        $ui.WriteLine("before sleep: " + $SESSION_ID)
        Method1
        $ui.WriteLine("sid: " + $SESSION_ID)
    } catch {
        $ui.WriteLine("error!" + $_.ToString())
    }
}
Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

Connect-PgletApp -Name account1/app1 -ScriptBlock {

    function main() {
        Invoke-Pglet "clean page"
        Invoke-Pglet "add text value='Enter your name:'"
        Invoke-Pglet "add textbox id=fullName value='someone'"
        Invoke-Pglet "add button primary id=submit text=Submit data=btn_event"
        
        Invoke-Pglet "set fullName value='John Smith'"
        
        while($true) {
            $e = Wait-PgletEvent
            if ($e.Target -eq 'submit' -and $e.Name -eq 'click') {
                greet
                return
            }
        }
    }
    
    function greet() {
        $fullName = Invoke-Pglet "get fullName value"
        Write-Host "Full name: $fullName"
    
        # output welcome message
        Invoke-Pglet "clean page"
        Invoke-Pglet "add text value='Hello, $fullName'"
        Invoke-Pglet "add button id=again text=Again"
    
        while($true) {
            $e = Wait-PgletEvent
            if ($e.Target -eq 'again' -and $e.Name -eq 'click') {
                main
                return
            }
        }
    }
    
    # entry point
    main
}
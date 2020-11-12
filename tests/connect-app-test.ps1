Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psm1'))

Connect-PgletApp -Name account1/app1 -ScriptBlock {

    function main() {
        Invoke-Pglet "clean page"
        #pglet "remove body"
        $rowId = Invoke-Pglet "add row id=body
          aaa=bbb"
        $colId = Invoke-Pglet "add col id=form to=$rowId"
        Invoke-Pglet "add text value='Enter your name:' to=$colId"
        Invoke-Pglet "add textbox id=fullName value='someone' to=$colId"
        Invoke-Pglet "add button id=submit text=Submit event=btn_event to=$colId"
        
        Invoke-Pglet "set body:form:fullName value='John Smith'"
        
        while($true) {
            $e = Wait-PgletEvent
            if ($e.Target -eq 'body:form:submit' -and $e.Name -eq 'click') {
                greet
                return
            }
        }
    }
    
    function greet() {
        $fullName = Invoke-Pglet "get body:form:fullName value"
        Write-Host "Full name: $fullName"
    
        # output welcome message
        Invoke-Pglet "clean page"
        Invoke-Pglet "add text value='Hello, $fullName'"
        Invoke-Pglet "add button id=again text=Again"
    
        while($true) {
            $e = Wait-PgletEvent $pageID
            if ($e.Target -eq 'again' -and $e.Name -eq 'click') {
                main
                return
            }
        }
    }
    
    # entry point
    main
}
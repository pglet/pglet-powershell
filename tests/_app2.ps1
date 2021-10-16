Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

Connect-PgletApp "index2" -Server "http://localhost:5000" -ScriptBlock {

    $page = $PGLET_PAGE
    $page.OnHashChange = {
        Write-Trace "Hash change: $($e.data)"
    }

    $page.OnClose = {
        Write-Trace "Session closed"
    }

    $txt1 = New-PgletText -Value "Line 101"
    $name = New-PgletTextbox -Label "Your name"
    
    $b = 10
    $btn1 = New-PgletButton -Text "-" -OnClick {
        $args[0].target
        $a = $b - 1
        Write-Host "Clicked! $a"
        #Start-Sleep -s 1
        "ddd"

        $name.value = ""
        $page.Update()
    }
    
    $btn2 = New-PgletButton -Text "Get results" -OnClick {
        $args[0].target
        $txt1.value = $name.Value
    
        $line = New-PgletText -Value "Hello, $($name.Value)"
    
        $idx = $page.controls.IndexOf($name)
        $page.controls.insert($idx + 1, $line)
    
        $name.value = ""
        $page.Update()
    }
    
    $page.Add($txt1, $name, $btn1, $btn2)
}
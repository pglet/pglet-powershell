Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

try {
    $page = Connect-PgletPage -Permissions ""
    $page.Clean()
    
    Write-Host $page.Url
    
    Write-Trace "This is trace!"
    
    $txt1 = Text -Value "Line 1"
    $name = Textbox -Label "Your name"
    
    $stack = Stack -Controls (Text -Value "Results")

    $nums = @{
        b = 10
    }
    
    $btn1 = Button -Text "---" -OnClick {
        $e | fl *
    
        $a = $nums.b - 1
        $nums.b = $a
        Write-Host "Clicked! $a"
        #Start-Sleep -s 1
        "ddd"
    }
    
    $btn2 = Button -Text "Get results" -OnClick {
        $e.target
    
        $a = $nums.b + 1
        $nums.b = $a
        Write-Host "Clicked! $a"
    
        $txt1.value = $name.Value
    
        $line = New-PgletText -Value "Hello, $($name.Value)"
    
        # $idx = $page.controls.IndexOf($name)
        # $page.controls.insert($idx + 1, $line)
    
        $stack.Controls.Clear()
        $stack.Controls.Add($line)
    
        $name.value = ""
        $page.Update()
    }
    
    $page.Add($txt1, $name, $btn1, $btn2, $stack)
    
    Switch-PgletEvents
} finally {
    Write-Host "Close pglet page!"
    Close-PgletPage
}


#Close-PgletPage


# while($true) {
#     $e = Wait-PgletEvent
#     Write-Host "$($e.target) $($e.name) $($e.data)"
#     #Start-Sleep -s 5
# }

#Disconnect-Pglet
Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

$page = Connect-PgletPage "index" -NoWindow

ipg "clean page"

Write-Trace "This is trace!"

$txt1 = Text -Value "Line 1"
$name = Textbox -Label "Your name"

$stack = Stack -Controls (Text -Value "Results")

$b = 10
$btn1 = Button -Text "-" -OnClick {
    $args[0].target
    $a = $b - 1
    Write-Host "Clicked! $a"
    #Start-Sleep -s 1
    "ddd"
}

$btn2 = Button -Text "Get results" -OnClick {
    $args[0].target
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


# while($true) {
#     $e = Wait-PgletEvent
#     Write-Host "$($e.target) $($e.name) $($e.data)"
#     #Start-Sleep -s 5
# }

#Disconnect-Pglet
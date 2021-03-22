Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

$page = Connect-PgletPage "index" -NoWindow

ipg "clean page"

$txt1 = New-PgletText -Value "Line 1"
$name = New-PgletTextbox -Label "Your name"

$b = 10
$btn1 = New-PgletButton -Text "-" -OnClick {
    $args[0].target
    $a = $b - 1
    Write-Host "Clicked! $a"
    #Start-Sleep -s 1
    "ddd"
}

$btn2 = New-PgletButton -Text "Get results" -OnClick {
    $args[0].target
    $txt1.value = $name.Value
    $page.Update()
}

$page.Add($txt1, $name, $btn1, $btn2)

Switch-PgletEvents


# while($true) {
#     $e = Wait-PgletEvent
#     Write-Host "$($e.target) $($e.name) $($e.data)"
#     #Start-Sleep -s 5
# }

#Disconnect-Pglet
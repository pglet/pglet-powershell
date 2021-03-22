Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

$page = Connect-PgletPage "index" -NoWindow

ipg "clean page"

$txt1 = New-PgletText -Value "Line 1"
$txt2 = New-PgletText -Value "Line 2"

$a = 1
$btn1 = New-PgletButton -Text "-" -OnClick {
    $a -= 1
    Write-Host "Clicked! $a"
    Start-Sleep -s 2
}

$btn2 = New-PgletButton -Text "+" -OnClick {
    $a += 1
    Write-Host "Clicked! $a"
    Start-Sleep -s 2
}

$page.Add($txt1, $txt2, $btn1, $btn2).Wait()

Switch-PgletEvent


# while($true) {
#     $e = Wait-PgletEvent
#     Write-Host "$($e.target) $($e.name) $($e.data)"
#     #Start-Sleep -s 5
# }

#Disconnect-Pglet
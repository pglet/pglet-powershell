Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

$page = Connect-PgletPage -Name "text-1" -NoWindow

try {
    $page.Clean()

    # multi-step task
    $prog2 = Progress -Label "Create new account" -Width "30%"
    $page.Add($prog2)

    $steps = @('Preparing environment...', 'Collecting information...', 'Performing operation...', 'Complete!')
    for($i = 0; $i -lt $steps.Length; $i++) {
        $prog2.Description = $steps[$i]
        $prog2.Value = 100 / ($steps.Length - 1) * $i
        $page.Update()
        Start-Sleep -Seconds 1
    }

} finally {
    $page.Close()
}
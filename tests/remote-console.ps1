Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

Connect-PgletApp -Name "remote-console" -ScriptBlock {
    $ErrorActionPreference = 'stop'

    $page = $PGLET_PAGE
    $page.Title = "PowerShell Remote Console"
    $page.HorizontalAlign = 'stretch'

    $cmd = TextBox -Width '100%'

    $view = @(
        Stack -Horizontal -Controls @(
            $cmd
            Button -Text "Run" -OnClick {
                Write-Trace "$($cmd.value)"
                $result = Invoke-Expression $cmd.value# | Out-String
                #$page.add((Text -Pre $result))
                $grid_results = Grid -Compact -Items $result
                $page.add($grid_results)
            }
        )
    )

    $page.add($view)
}
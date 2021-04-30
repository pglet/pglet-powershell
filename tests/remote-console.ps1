Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

Connect-PgletApp -Name "remote-console" -ScriptBlock {
    $ErrorActionPreference = 'stop'

    $page = $PGLET_PAGE
    $page.Title = "PowerShell Remote Console"
    $page.HorizontalAlign = 'stretch'

    $cmd = TextBox -Placeholder "Type PowerShell command and click Run or press ENTER..." -Width '100%'

    $run_on_click = {
        $cmd_text = $cmd.value
        if ([string]::IsNullOrWhitespace($cmd_text)) {
            return
        }

        $cmd.value = ''
        $command_panel.disabled = $true
        $results.controls.insert(0, (Text $cmd_text -BgColor '#eee' -Padding 10))
        $results.controls.insert(1, (Spinner))

        $page.update()

        try {
            $result = Invoke-Expression $cmd_text

            if ($result -is [System.Array]) {
                $result_control = Grid -Compact -Items $result
            } else {
                $result_control = Text -Value ($result | Out-String) -Pre -Padding 10
            }
        } catch {
            $result_control = Text -Value "$_" -Pre -Padding 10 -Color 'red'
        }

        $command_panel.disabled = $false
        $results.controls.removeAt(1)
        $results.controls.insert(1, $result_control)
        $page.update()
    }
    
    $command_panel = Stack -Horizontal -OnSubmit $run_on_click -Controls @(
        $cmd
        Button -Text "Run" -Primary -Icon 'Play' -OnClick $run_on_click
    )

    # results container
    $results = Stack

    $view = @(
        $command_panel
        Stack -Controls @(
            Stack -Horizontal -VerticalAlign Center -Controls @(
                Text 'Results' -Size large
                Button -Icon 'Clear' -Title 'Clear results' -OnClick {
                    $results.controls.clear()
                    $results.update()
                }
            )
            $results
        )
    )

    $page.add($view)
}
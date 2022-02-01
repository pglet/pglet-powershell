Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

Connect-PgletApp -Name "remote-console" -ScriptBlock {
    $ErrorActionPreference = 'stop'

    $page = $PGLET_PAGE
    $page.Title = "PowerShell Remote Console"
    #$page.Theme = 'dark'
    #$page.themePrimaryColor = '#8b95f7'
    $page.HorizontalAlign = 'stretch'

    # Textbox with a command entered
    $cmd = TextBox -Placeholder "Type PowerShell command and click Run or press ENTER..." -Width '100%'

    # Event handler to call when "Run" button is clicked or Enter pressed
    $run_on_click = {
        $cmd_text = $cmd.value
        if ([string]::IsNullOrWhitespace($cmd_text)) {
            return
        }

        # disable textbox and Run button, add spinner while the command is evaluating
        $cmd.value = ''
        $command_panel.disabled = $true
        $results.controls.insert(0, (Text $cmd_text -BgColor 'neutralLight' -Padding 5))
        $results.controls.insert(1, (Spinner))
        $page.update()

        try {

            # run the command
            $result = Invoke-Expression $cmd_text

            # if result is Array present it as Grid; otherwise Text
            if ($result -is [System.Array]) {
                $result_control = Grid -Compact -Items $result
            } else {
                $result_control = Text -Value ($result | Out-String) -Pre -Padding 5
            }
        } catch {
            $result_control = Text -Value "$_" -Pre -Padding 10 -Color 'Red10'
        }

        # re-enable controls and replace spinner with the results
        $command_panel.disabled = $false
        $results.controls.removeAt(1)
        $results.controls.insert(1, $result_control)
        $page.update()
    }
    
    # container for command textbox and Run button
    $command_panel = Stack -Horizontal -OnSubmit $run_on_click -Controls @(
        $cmd
        Button -Text "Run" -Primary -Icon 'Play' -OnClick $run_on_click
    )

    # results container
    $results = Stack

    # "main" view combining all controls together
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

    # display the "main" view onto the page
    $page.add($view)
}
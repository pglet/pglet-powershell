function Write-Trace {
    param(
        [Parameter(Mandatory=$true, Position=0, ValueFromRemainingArguments=$true)]
        [string]$value
    )
    [System.Console]::WriteLine($value)
}

New-Alias -Name BarChart -Value New-PgletBarChart
New-Alias -Name BarChartDataPoint -Value New-PgletBarChartDataPoint
New-Alias -Name Button -Value New-PgletButton
New-Alias -Name Callout -Value New-PgletCallout
New-Alias -Name Checkbox -Value New-PgletCheckbox
New-Alias -Name ChoiceGroup -Value New-PgletChoiceGroup
New-Alias -Name ChoiceGroupOption -Value New-PgletChoiceGroupOption
New-Alias -Name Dialog -Value New-PgletDialog
New-Alias -Name Dropdown -Value New-PgletDropdown
New-Alias -Name DropdownOption -Value New-PgletDropdownOption
New-Alias -Name GridColumn -Value New-PgletGridColumn
New-Alias -Name Grid -Value New-PgletGrid
New-Alias -Name Icon -Value New-PgletIcon
New-Alias -Name Image -Value New-PgletImage
New-Alias -Name LineChart -Value New-PgletLineChart
New-Alias -Name LineChartData -Value New-PgletLineChartPoint
New-Alias -Name LineChartDataPoint -Value New-PgletLineChartDataPoint
New-Alias -Name Link -Value New-PgletLink
New-Alias -Name MenuItem -Value New-PgletMenuItem
New-Alias -Name MessageButton -Value New-PgletMessageButton
New-Alias -Name Message -Value New-PgletMessage
New-Alias -Name NavItem -Value New-PgletNavItem
New-Alias -Name Nav -Value New-PgletNav
New-Alias -Name Panel -Value New-PgletPanel
New-Alias -Name PieChart -Value New-PgletPieChart
New-Alias -Name PieChartDataPoint -Value New-PgletPieChartDataPoint
New-Alias -Name Progress -Value New-PgletProgress
New-Alias -Name SearchBox -Value New-PgletSearchBox
New-Alias -Name Slider -Value New-PgletSlider
New-Alias -Name SpinButton -Value New-PgletSpinButton
New-Alias -Name Stack -Value New-PgletStack
New-Alias -Name Tab -Value New-PgletTab
New-Alias -Name Tabs -Value New-PgletTabs
New-Alias -Name TextBox -Value New-PgletTextBox
New-Alias -Name Text -Value New-PgletText
New-Alias -Name Toggle -Value New-PgletToggle
New-Alias -Name Toolbar -Value New-PgletToolbar
New-Alias -Name VerticalBarChart -Value New-PgletVerticalBarChart
New-Alias -Name VerticalBarChartDataPoint -Value New-PgletVerticalBarChartDataPoint
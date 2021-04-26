Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

Connect-PgletApp -Name "pglet-datepicker" -ScriptBlock {
  $ErrorActionPreference = 'stop'

  Write-Trace "session started!"

  $d = Get-Date

  #try {
    $controls = @(
      DatePicker -Label 'Start date' -Value $d
      DatePicker -Label 'End date'
      DatePicker -Label 'Allow text input' -AllowTextInput
      DatePicker -Label 'Allow text input with placeholder' -Placeholder 'Select date...' -AllowTextInput  -Width '50%'
      DatePicker -Value '01/01/2012' -Label 'Required' -Placeholder 'Select date...' -Required -AllowTextInput -Width '50%'
    )
  
    $pglet_page.add($controls)

    Start-Sleep -s 3

    $controls = @(
      DatePicker -Value '15/15/2012'
    )
  
    $pglet_page.add($controls)    
  # }
  # catch {
  #     Write-Trace $_
  # }


}
Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

Connect-PgletApp -Name "app-1" -ScriptBlock {
  $pglet_page.Add((Text -Id MyText -Value "Started: $(Get-Date), Session ID: $($pglet_page.SessionID)"))
}
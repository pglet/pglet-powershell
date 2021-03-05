Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'src', 'Pglet.PowerShell', 'bin', 'Debug', 'netstandard2.0', 'Pglet.PowerShell.dll'))

$page = Connect-PgletPage "index" -NoWindow

$page.Connection.Send("add text value='aaa'")
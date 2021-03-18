Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

$page2 = Connect-PgletPage "index" -NoWindow

ipg "clean page"

Invoke-Pglet "add text id=title value='Example 1' size=xxLarge"
Invoke-Pglet "add text id=subTitle value='Sub Example 1' size=xLarge"

Invoke-Pglet "set title value='Example 2'"

Start-Sleep -s 5

# Invoke-Pglet "add text value='Batch' size=xxLarge"
# Invoke-Pglet "add text value='Line1'"
# Invoke-Pglet "add text value='Line2'"

$commands = @(
    "set title value='Example 3!'"
    "add text value='Batch' size=xxLarge"
    "add text value='Line1'"
    "add text value='Line2'"
    "remove subTitle"
)

Invoke-Pglet $commands
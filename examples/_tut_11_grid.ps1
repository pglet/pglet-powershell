Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

$page = Connect-PgletPage -Name "grid-1"

try {
    $page.Clean()

    $items = @(
      @{
        "First name" = "John"
        "Last name" = "Smith"
      }
      @{
        "First name" = "Alice"
        "Last name" = "Brown"
      }
    )

    $grid = Grid -Items $items
    $page.Add($grid)

} finally {
    $page.Close()
}
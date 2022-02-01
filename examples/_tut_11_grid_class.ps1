Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

$page = Connect-PgletPage -Name "grid-1"

try {
    $page.Clean()

    class Person {
        [string]$FirstName
        [string]$LastName
      
        Person($firstName, $lastName) {
            $this.FirstName = $firstName
            $this.LastName = $lastName
        }
    }

    $items = @(
        [Person]::new('John', 'Smith')
        [Person]::new('Alice', 'Brown')
    )
    $grid = Grid -Items $items -Columns @(
        GridColumn -Name "First name" -FieldName "FirstName" -Sortable "string"
        GridColumn -Name "Last name" -FieldName "LastName" -Sortable "string"
    )
    $page.Add($grid)
}
finally {
    $page.Close()
}
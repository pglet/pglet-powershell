Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

$page = Connect-PgletPage "index" -NoWindow
Invoke-Pglet "clean page"

class Person {
  [string]$FirstName
  [string]$LastName
  [int]$Age

  Person($firstName, $lastName, $age) {
    $this.FirstName = $firstName
    $this.LastName = $lastName
    $this.Age = $age
  }

  [string]ToString() {
      return ("{0}|{1}|{2}" -f $this.FirstName, $this.LastName, $this.Age)
  }
}

$grid = Grid -SelectionMode multiple -Columns @(
  GridColumn -FieldName "FirstName" -Sortable 'true'
  GridColumn -Name "Last name" -FieldName "LastName"
  GridColumn -Name "Age" -FieldName "Age" -Sortable 'number'
) -Items @(
  [Person]::new('John', 'Smith', 43)
  [Person]::new('Alice', 'Brown', 32)
) -OnSelect {
  foreach($item in $e.control.SelectedItems) {
    Write-Trace $item
  }
}

$data = @{
  n = 1
}

$add_btn = Button -Text 'Add new item' -OnClick {
  $n = $data.n
  $grid.items.removeAt(0)
  $grid.items.add([Person]::new("First $n", "Last $n", $n + 20))
  $grid.update()
  $data.n++
}

$page.Add($grid, $add_btn)

Switch-PgletEvents
Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

Connect-PgletApp "ps-grid-edit-example" -ScriptBlock {

  class Person {
    [string]$FirstName
    [string]$LastName
    [int]$Age
    [bool]$Vaccinated
  
    Person($firstName, $lastName, $age, $vaccinated) {
      $this.FirstName = $firstName
      $this.LastName = $lastName
      $this.Age = $age
      $this.Vaccinated = $vaccinated
    }
  
    [string]ToString() {
        return ("{0}|{1}|{2}|{3}" -f $this.FirstName, $this.LastName, $this.Age, $this.Vaccinated)
    }
  }

  $delete_btn = MenuItem -Icon "Delete" -Text "Delete selected" -Disabled -OnClick {
    # TODO
  }

  # Toolbar with action buttons
  $toolbar = Toolbar -Items @(
    MenuItem -Icon "Add" -Text "New record" -OnClick {
      $dialog.open = $true
      $pglet_page.update()
    }
    $delete_btn
  )
  
  # Grid to display items
  $grid = Grid -SelectionMode multiple -Columns @(
    GridColumn -FieldName "FirstName" -Sortable 'true' -Resizable -MinWidth 200 -MaxWidth 300 -TemplateControls @(
      TextBox -Value "{FirstName}"
    )
    GridColumn -Name "Last name" -Resizable -MinWidth 200 -MaxWidth 300 -TemplateControls @(
      TextBox -Value "{LastName}"
    )
    GridColumn -Name "Age" -FieldName "Age" -Resizable -MinWidth 100 -MaxWidth 200 -Sortable 'number' -TemplateControls @(
      TextBox -Value "{Age}"
    )
    GridColumn -Name "Vaccinated" -Resizable -TemplateControls @(
      CheckBox -ValueField "Vaccinated"
    )    
  ) -Items @(
    [Person]::new('John', 'Smith', 43, $true)
    [Person]::new('Alice', 'Brown', 32, $true)
  ) -OnSelect {
    $delete_btn.Disabled = if ($e.control.SelectedItems.Count -eq 0) { $true } else { $false }
    $pglet_page.Update()
    # foreach($item in $e.control.SelectedItems) {
    #   Write-Trace $item
    # }
  }

  # Dialog to add new item
  $firstName = TextBox -Label "First name"
  $lastName = TextBox -Label "Last name"
  $age = SpinButton -Label "Age" -Value 20
  $vaccinated = CheckBox -Label "Vaccinated" -BoxSide End

  $dialog = Dialog -Type Close -Title "Add new item" -Controls @(
      Stack -Gap 10 -Controls @(
        $firstName
        $lastName
        $age
        $vaccinated
      )
    )

  $addButton = Button "Add" -Primary -OnClick {
    $dialog.open = $false
    $pglet_page.update()
  }

  $cancelButton = Button "Cancel" -OnClick {
    $dialog.open = $false
    $pglet_page.update()
  }

  $dialog.FooterControls.add($addButton)
  $dialog.FooterControls.add($cancelButton)

  # add all elements to the page
  $pglet_page.Add($toolbar, $grid, $dialog)
}
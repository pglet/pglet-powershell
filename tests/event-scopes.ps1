Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

function Func1() {
  Write-Host "`n`n====== Func1 ======"
  $aaaaaaaaa = 1
  Set-Variable -Name "bbbbbbbbbbb" -Value "222"
  #Get-Variable

  function Func2() {
    Write-Host "`n`n====== Func2 ======"
    $ccccccccc = 333333
    $cccc1 = "31313131"

    function Func3() {
      Write-Host "`n`n====== Func3 ======"
      Write-Host "$ccccccccc"
      $dddddddd = 444444444
      Write-Host "`n`n====== Scope 0 ======"
      Get-Variable -Scope 0
      Write-Host "`n`n====== Scope 1 ======"
      Get-Variable -Scope 1
      Write-Host "`n`n====== Scope 2 ======"
      Get-Variable -Scope 2
      Write-Host "`n`n====== Scope 3 ======"
      Get-Variable -Scope 3
      Write-Host "`n`n====== Scope 4 ======"
      Get-Variable -Scope 4
      Write-Host "`n`n====== Scope 5 ======"
      Get-Variable
      (Get-Variable).Count
      (Get-Variable -Scope Global).Count
    }

    Func3
  }

  Func2
}

Func1

Write-Host "`n`n====== Global ======"
#Get-Variable

return




Connect-PgletApp -Name "pglet-textbox" -ScriptBlock {

  function textboxWithOnChange() {
    $displayedText = Text
    $enteredText = TextBox -Label "With onChange event" -OnChange {
      $displayedText.value = $enteredText.value
      $stack.update()
    }

    $stack = Stack -Controls @(
      $enteredText
      $displayedText
    )

    $enteredText.OnChange = {
      Write-Host "abc!"
    }

    function inner2() {
      $aaaaaa = 333
      $txt = TextBox -OnChange {
        Write-Host "Another change!"
      }
    }    

    inner2

    return $stack    
  }

  $controls = Stack -Gap 20 -Controls @(
    Button "Click me!" -OnClick {
      Write-Trace "Clicked!"
    }
  )

  $controls.controls.add((textboxWithOnChange))

  $pglet_page.add($controls)
}
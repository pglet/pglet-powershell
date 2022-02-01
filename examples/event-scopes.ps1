Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

Connect-PgletApp -Name "pglet-textbox" -ScriptBlock {

  $txt2 = Text

  function textboxWithOnChange() {
    $displayedText = Text
    $enteredText = TextBox -Label "With onChange event"

    $stack = Stack -Controls @(
      $enteredText
      $displayedText
    )

    $enteredText.OnChange = {
      $displayedText.value = $enteredText.value
      $stack.update()
    }

    function inner2() {
      $aaaaaa = 333
      $txt = TextBox -Label "Another with onchange" -OnChange {
        $txt2.value = $e.control.value
        $pglet_page.update()
      }
      $pglet_page.add($txt)
    }    

    inner2

    return $stack    
  }

  $textbox_with_change_contols = textboxWithOnChange

  $controls = Stack -Gap 20 -Controls @(
    Button "Click me!" -OnClick {
      Write-Trace "====== Clicked! ======="
      Write-Trace "$textbox_with_change_contols"
      foreach ($key in $args[0].keys) {
        Write-Trace "$key=$($args[0][$key])"
      }
    }
  )

  $controls.controls.add($textbox_with_change_contols)

  $pglet_page.add($controls, $txt2)
}
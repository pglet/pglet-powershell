Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

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
    return $stack    
  }

  $controls = Stack -Gap 20 -Controls @(
    TextBox -Multiline -AutoAdjustHeight -Label "Multiline textbox with auto-adjust height"
    TextBox -Underlined -Label "Underlined textbox:"
    TextBox -Borderless -Label "Borderless textbox"
    TextBox -Prefix 'https://' -Label "Textbox with prefix"
    TextBox -Suffix 'px' -Label "Textbox with sufix"
    TextBox -Prefix 'https://' -Suffix '.com' -Label "Textbox with prefix and suffix"
  )

  $controls.controls.add((textboxWithOnChange))

  $pglet_page.add($controls)
}
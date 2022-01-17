Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

Connect-PgletApp -Name "app-1" -ScriptBlock {
    $page = $PGLET_PAGE

    $txt_name = TextBox -Label 'What is your name?'
    $btn_hello = Button -Primary -Text 'Say hello' -OnClick {
      $page.Clean()
      $page.Add(((Text -Value "Hello, $($txt_name.value)!")))
      $page.Update()
    }
  
    $page.Add($txt_name, $btn_hello)
}
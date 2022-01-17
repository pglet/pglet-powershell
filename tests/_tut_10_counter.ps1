Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

Connect-PgletApp -Name "app-1" -ScriptBlock {
  $page = $PGLET_PAGE

  $num_txt = TextBox -Value 0

  $minus_btn = Button "-" -OnClick {
    $num_txt.Value = [int]$num_txt.Value - 1
    $page.Update()
  }

  $plus_btn = Button "+" -OnClick {
    $num_txt.Value = [int]$num_txt.Value + 1
    $page.Update()
  }

  $page.Add((Stack -Horizontal -Controls @(
    $minus_btn
    $num_txt
    $plus_btn
  )))
}
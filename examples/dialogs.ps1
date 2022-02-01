Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

Connect-PgletApp -Name "pglet-dialog" -ScriptBlock {
  $ErrorActionPreference = 'stop'

  $page = $pglet_page

  # Basic dialog
  $basicDialog = Dialog -Type LargeHeader -Title "Missing subject" `
    -SubText "Do you want to send this message without a subject?" -FooterControls @(
      Button "Yes" -Primary
      Button "No"
    )

  $openBasic = Button -Text 'Open basic dialog' -OnClick {
    $basicDialog.open = $true
    $basicDialog.update()
  }

  # Dialog with the body
  $dialog = Dialog -Type Close -Title "Color" `
    -SubText "What is your favourite color?" -Controls @(
      ChoiceGroup -Options @(
        ChoiceGroupOption -Text "Red"
        ChoiceGroupOption -Text "Green"
        ChoiceGroupOption -Text "Blue"
      )
    )

  $yesButton = Button "Yes" -Primary -OnClick {
    $dialog.open = $false
    $page.update()
  }

  $noButton = Button "No" -OnClick {
    $dialog.open = $false
    $page.update()
  }

  $dialog.FooterControls.add($yesButton)
  $dialog.FooterControls.add($noButton)
  $dialog.update()

  $open = Button -Text 'Open dialog' -OnClick {
    $dialog.open = $true
    $page.update()
  }  
  
  $page.add(
    $openBasic,
    $basicDialog,
    $open,
    $dialog
  )
}
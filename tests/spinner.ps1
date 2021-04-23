Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

Connect-PgletApp -Name "pglet-spinner" -ScriptBlock {

  $controls = @(
    Text "Spinner sizes" -Size xLarge
    Spinner -Label "Extra small spinner" -Size xSmall -LabelPosition Left 
    Spinner -Label "Small spinner" -Size small -LabelPosition Left 
    Spinner -Label "Medium spinner" -Size medium -LabelPosition Left 
    Spinner -Label "Large spinner" -Size large -LabelPosition Left 

    Text "Spinner label positioning" -Size xLarge

    Text "Spinner with label positioned below"
    Spinner -Label "I am definitely loading..." -LabelPosition Bottom

    Text "Spinner with label positioned above"
    Spinner -Label "Seriously, still loading..." -LabelPosition Top

    Text "Spinner with label positioned to right"
    Spinner -Label "Wait, wait..." -LabelPosition Right

    Text "Spinner with label positioned to left"
    Spinner -Label "Nope, still loading..." -LabelPosition Left
  )
  
  $pglet_page.add($controls)
}
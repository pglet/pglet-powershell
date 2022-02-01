Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

Connect-PgletPage -Name "index" -Token 1234 -NoWindow

function Stack() {
  "stack"
}

function Button() {
  "button"
}

$stack = Stack -Horizontal -Controls @(
  Button -Text "7"
  Button -Text "8"
  Button -Text "9"
  Button -Text "10"
)

$stack.Controls.Add($b)
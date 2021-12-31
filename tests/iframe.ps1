Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

Connect-PgletApp -ScriptBlock {
    $pglet_page.title = "IFrame example"
    $pglet_page.add(
        (IFrame -Src 'https://pglet.io' -Width '100%' -Height 300 -Border '2px solid red')
    )
}
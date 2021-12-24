Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

Connect-PgletApp -ScriptBlock {
    $page = $PGLET_PAGE

    $page.title = "IFrame example"
    $page.add(
        (IFrame -Src 'https://pglet.io' -Width '100%' -Height 300 -Border '2px solid red')
    )
}
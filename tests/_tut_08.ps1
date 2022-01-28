Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

$page = Connect-PgletPage -Name "page-4" -NoWindow

try {
    $page.Clean()

    $yes_btn = Button -Text "Yes" -Primary
    $no_btn = Button -Text "No"
    $confirm_dialog = Stack -Horizontal -Controls @($yes_btn, $no_btn)

    # display confirm buttons
    $page.Add(@(
        Text "Do you want to proceed?"
        $confirm_dialog
    ))
    
    $e = Wait-PgletEvent

    # remove buttons from a page
    $page.Controls.Remove($confirm_dialog)

    # display message
    if ($e.Control -eq $yes_btn) {
        $page.Add((Text "YES button was clicked"))
    } else {
        $page.Add((Text "NO button was clicked"))
    }

} finally {
    $page.Close()
}

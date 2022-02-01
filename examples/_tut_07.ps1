Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

$page = Connect-PgletPage -Name "page-3" -NoWindow

try {
    # Unique identifier for this host client
    $hostname = [Environment]::MachineName

    # add host-unique stack to put on it all other controls
    $st = Stack -Id "st_$hostname"
    $page.Add($st)
    
    # cleanup stack after adding to a page
    $st.Clean()
    
    # add controls to the stack without unique ID
    $st.Controls.Add((Text -Value "$hostname - $(Get-Date)"))
    $page.Update()
} finally {
    $page.Close()
}

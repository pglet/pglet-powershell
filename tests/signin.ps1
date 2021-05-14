Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

Connect-PgletApp "signin-test" -NoWindow -ScriptBlock {
    $page = $PGLET_PAGE

    $page.onDismissSignin = {
        Write-Trace "Signin cancelled"
    }

    $currentUser = Text
        
    $signinButton = Button -Text "Sign in" -OnClick {
        Write-Trace "Display signin dialog"
        $page.signin = "*"
        $page.signinGroups = $true
        $page.signinAllowDismiss = $true
        $page.update()
    }

    $signoutButton = Button -Text "Sign out" -OnClick {
        $page.connection.send("signout")
    }

    $page.onSignin = {
        Write-Trace "Signed in!"
        updateCurrentUser
        $page.update()
    }

    $page.onSignout = {
        Write-Trace "Signed out!"
        updateCurrentUser
        $page.update()
    }

    function updateCurrentUser() {
        if ($page.userName) {
            # signed in
            $currentUser.value = "Welcome back, $($page.userName)"
            $signoutButton.Visible = $true
            $signinButton.Visible = $false
        } else {
            # anonymous
            $currentUser.value = "Not logged in"
            $signinButton.Visible = $true
            $signoutButton.Visible = $false`
        }
    }

    updateCurrentUser
    
    $page.add($currentUser, $signinButton, $signoutButton)
}
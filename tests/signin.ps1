Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

Connect-PgletApp "signin-test" -Local -NoWindow -ScriptBlock {
    $page = $PGLET_PAGE

    $page.onDismissSignin = {
        Write-Trace "Signin cancelled"
    }

    $currentUser = Text
        
    $signinButton = Button -Primary -Text "Sign in" -OnClick {
        Write-Trace "Display signin dialog"

        try {
            $success = Show-PgletSignin -AuthProviders "github" -AuthGroups -AllowDismiss
            if ($success) {
                Write-Trace "Signed in!"
                updateCurrentUser
                $page.update()
            }
        } catch {
            Write-Trace "$_"
        }        
    }

    $signoutButton = Button -Primary -Text "Sign out" -OnClick {
        $page.connection.send("signout")
    }

    $page.onSignout = {
        Write-Trace "Signed out!"
        updateCurrentUser
        $page.update()
    }

    $checkAnon = Button -Text "Check anonymous access" -OnClick {
        $result = $page.canAccess("")
        Write-Trace $result
    }    

    $checkAnyAuth = Button -Text "Check any login" -OnClick {
        $result = $page.canAccess("*")
        Write-Trace $result
    }

    $checkGitHubTeams = Button -Text "Check GitHub permissions" -OnClick {
        $result = $page.canAccess("github:pglet/core developers")
        Write-Trace $result
    }

    $checkGoogleLogin = Button -Text "Check Google login" -OnClick {
        $result = $page.canAccess("google:*@appveyor.com")
        Write-Trace $result
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

    $page.ThemePrimaryColor = '#cc73ff'
    $page.ThemeTextColor = '#e1e4e8'
    $page.ThemeBackgroundColor = '#24292e'
    
    $page.add($currentUser, $signinButton, $signoutButton, $checkAnon, $checkAnyAuth, $checkGitHubTeams, $checkGoogleLogin)
}
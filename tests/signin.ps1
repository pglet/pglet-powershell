Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

Connect-PgletApp "signin-test" -NoWindow -ScriptBlock {
    $page = $PGLET_PAGE

    $page.onSignin = {
        Write-Trace "Signed in!"
        Write-Trace $page.userLogin
    }

    $page.onDismissSignin = {
        Write-Trace "Signin cancelled"
    }    
        
    $signinButton = Button -Text "Sign in" -OnClick {
        Write-Trace "Click!"
        Write-Trace "ssss$($page.signin)"
        $page.signin = "*"
        $page.signinAllowDismiss = $true
        $page.update()
    }
    
    $page.add($signinButton)
}
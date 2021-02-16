@{
    RootModule        = 'pglet.psm1'
    ModuleVersion     = '0.1.0'
    GUID              = '6d0d7541-197f-4cd4-9655-6928a25c6b13'
    Author            = 'AppVeyor Systems Inc.'
    CompanyName       = 'AppVeyor Systems Inc.'
    Copyright         = 'Copyright (c) 2021 Appveyor Systems Inc. All rights reserved.'
    Description       = 'Pglet client for PowerShell - easily create rich and responsive web apps in PowerShell.'
    PowerShellVersion = '5.0'
    FunctionsToExport = @(
        'Connect-PgletApp',
        'Connect-PgletPage',
        'Disconnect-Pglet',
        'Invoke-Pglet',
        'Wait-PgletEvent',
        'Write-Trace'
        )
    CmdletsToExport   = '*'
    VariablesToExport = '*'
    AliasesToExport   = @(
        'ipg'
        )
    PrivateData       = @{
        Pglet  = @{
            MinimumVersion = "0.2.2"
        }
        PSData = @{
            #Tags = @()
            LicenseUri = 'https://github.com/pglet/pglet-powershell/blob/main/LICENSE'
            ProjectUri = 'https://github.com/pglet/pglet-powershell'
            IconUri = 'https://pglet.io/img/pglet-logo-300.png'
    
            # ReleaseNotes of this module
            # ReleaseNotes = ''
    
            # Prerelease string of this module
            # Prerelease = 'beta'
        }
    } # End of PrivateData hashtable
    # HelpInfoURI = ''
    # DefaultCommandPrefix = ''
}
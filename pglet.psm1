<#

References:
https://learn-powershell.net/2013/04/19/sharing-variables-and-live-objects-between-powershell-runspaces/

Default session variables:

  PGLET_CONNECTION_ID   - the last page connection ID.
  PGLET_CONTROL_ID      - the last added control ID.
  PGLET_EVENT_TARGET    - the last received event target (control ID).
  PGLET_EVENT_NAME      - the last received event name.
  PGLET_EVENT_DATA      - the last received event data.

#>

$global:PGLET_CONNECTIONS = [hashtable]::Synchronized(@{})
$global:PGLET_CONNECTION_ID = ""

function Connect-PgletApp {
    [CmdletBinding()]
    param
    (
      [Parameter(Mandatory = $false, Position = 0, HelpMessage = "The name of Pglet app.")]
      [string]$Name,

      [Parameter(Mandatory = $true, HelpMessage = "A handler script block for a new user session.")]
      [scriptblock]$ScriptBlock,

      [Parameter(Mandatory = $false, HelpMessage = "Makes the app available as public at pglet.io hosted service.")]
      [switch]$Public,

      [Parameter(Mandatory = $false, HelpMessage = "Makes the app available as private at pglet.io hosted service.")]
      [switch]$Private,      

      [Parameter(Mandatory = $false, HelpMessage = "Connects to the app on a self-hosted Pglet server.")]
      [string]$Server,

      [Parameter(Mandatory = $false, HelpMessage = "Authentication token for pglet.io service or a self-hosted Pglet server.")]
      [string]$Token      
    )

    $ErrorActionPreference = "Stop"

    $pargs = @()
    $pargs += "app"
    if ($Name) {
        $pargs += $Name
    } else {
        $pargs += "*"
    }

    if ($Public.IsPresent) {
        $pargs += "--public"
    }
    elseif ($Private.IsPresent) {
        $pargs += "--private"
    }

    if ($Server) {
        $pargs += "--server"
        $pargs += $Server
    }

    if ($Token) {
        $pargs += "--token"
        $pargs += $Token
    }    

    $Sessions = [hashtable]::Synchronized(@{})

    $sessionsMonitor = {
        function Write-Trace($value) {
            [System.Console]::WriteLine($value)
        }

        try {
            while ($true) {
                $sids = @()
                $sids += $Sessions.Keys
                foreach($sid in $sids) {
                    $session = $Sessions[$sid]
                    if ($session.AsyncHandler.IsCompleted) {
                        try {
                            Write-Trace "Session exited: $sid"
                            $dc = $session.PowerShell.EndInvoke($session.AsyncHandler)
                        }
                        catch {
                            Write-Trace $dc.Count
                        }
                        $session.Runspace.Close()
                        $session.PowerShell.Dispose()
                        $Sessions.Remove($sid)
                    }
                }
                Start-Sleep -s 2
            }
        }
        catch {
            Write-Trace "An error occurred: $_"
        }
        finally {
            Write-Trace "Monitor stopped"
        }
    }

    # sessions monitor
    $rsMonitor = [runspacefactory]::CreateRunspace()
    $psMonitor = [powershell]::Create()
    $psMonitor.Runspace = $rsMonitor
    $rsMonitor.Open() | Out-Null
    $rsMonitor.SessionStateProxy.SetVariable('Sessions', $Sessions)
    $psMonitor.AddScript($sessionsMonitor) | Out-Null
    $psMonitor.BeginInvoke() | Out-Null

    try {
        pglet.exe $pargs | ForEach-Object {

            $SessionID = $_
            $Runspace = [runspacefactory]::CreateRunspace()
            $PowerShell = [powershell]::Create()
            $PowerShell.Runspace = $Runspace
            $Runspace.Open() | Out-Null
            $PowerShell.AddScript("Import-Module ([IO.Path]::Combine('$PSScriptRoot', 'pglet.psm1'))") | Out-Null
            $PowerShell.AddScript('$global:PGLET_CONNECTION_ID="' + $SessionID + '"') | Out-Null
            $PowerShell.AddScript($ScriptBlock) | Out-Null
    
            # add session to monitor
            $Sessions[$SessionID] = @{
                SessionID = $SessionID
                PowerShell = $PowerShell
                Runspace = $Runspace
                AsyncHandler = $PowerShell.BeginInvoke()
            }
        }
    }
    finally {
        Write-Host "Script ended!"

        # terminate all running runspaces
        $sids = @()
        $sids += $Sessions.Keys

        foreach($sid in $sids) {
            $session = $Sessions[$sid]
            if (-not $session.AsyncHandler.IsCompleted) {
                Write-Host "Terminate session:" $sid
                $session.PowerShell.Stop()
                $session.Runspace.Close()
                $session.PowerShell.Dispose()
                $Sessions.Remove($sid)
            }
        }

        $rsMonitor.Close()
        $psMonitor.Dispose()
    }
}

function Connect-PgletPage {
    [CmdletBinding()]
    param
    (
      [Parameter(Mandatory = $false, Position = 0, HelpMessage = "The name of Pglet page.")]
      [string]$Name,

      [Parameter(Mandatory = $false, HelpMessage = "Makes the page available as public at pglet.io service or a self-hosted Pglet server.")]
      [switch]$Public,

      [Parameter(Mandatory = $false, HelpMessage = "Makes the page available as private at pglet.io service or a self-hosted Pglet server.")]
      [switch]$Private,      

      [Parameter(Mandatory = $false, HelpMessage = "Connects to the page on a self-hosted Pglet server.")]
      [string]$Server,

      [Parameter(Mandatory = $false, HelpMessage = "Authentication token for pglet.io service or a self-hosted Pglet server.")]
      [string]$Token      
    )

    $ErrorActionPreference = "Stop"

    $pargs = @()
    $pargs += "page"
    if ($Name) {
        $pargs += $Name
    } else {
        $pargs += "*"
    }

    if ($Public.IsPresent) {
        $pargs += "--public"
    }
    elseif ($Private.IsPresent) {
        $pargs += "--private"
    }

    if ($Server) {
        $pargs += "--server"
        $pargs += $Server
    }

    if ($Token) {
        $pargs += "--token"
        $pargs += $Token
    }

    $global:PGLET_CONNECTION_ID = (pglet.exe $pargs)

    
}

function Disconnect-Pglet {
    [CmdletBinding()]
    param
    (
        [Parameter(Mandatory = $false, Position = 0, HelpMessage = "Page connection ID.")]
        [string]$Page
    )

    $ErrorActionPreference = "Stop"

    if ($global:PGLET_CONNECTIONS) {
        Write-Host "Close Pglet connections"
    }
}

function Invoke-Pglet {
    [CmdletBinding()]
    param
    (
      [Parameter(Mandatory = $true, Position = 0, HelpMessage = "Pglet command to send.")]
      [string]$Command,

      [Parameter(Mandatory = $false, Position = 1, HelpMessage = "Page connection ID.")]
      [string]$Page
    )

    $ErrorActionPreference = "Stop"

    Write-Trace "Default Page ID: $PGLET_CONNECTION_ID"
    Write-Trace "Page ID: $Page"
    Write-Trace "Command: $Command"
}

function Wait-PgletEvent() {
    [CmdletBinding()]
    param
    (
        [Parameter(Mandatory = $false, Position = 0, HelpMessage = "Page connection ID.")]
        [string]$Page
    )

    $ErrorActionPreference = "Stop"
}

function Write-Trace {
    param(
        [Parameter(Mandatory=$true, Position=0, ValueFromRemainingArguments=$true)]
        [string]$value
    )
    [System.Console]::WriteLine($value)
}

New-Alias -Name ipg -Value Invoke-Pglet

# Exported functions
Export-ModuleMember -Function Connect-PgletApp, Connect-PgletPage, Disconnect-Pglet, Invoke-Pglet, Wait-PgletEvent, Write-Trace -Alias ipg
<#

References:
https://learn-powershell.net/2013/04/19/sharing-variables-and-live-objects-between-powershell-runspaces/

#>
function Connect-PgletApp {
    [CmdletBinding()]
    param
    (
      [Parameter(Mandatory = $false, Position = 0, HelpMessage = "The name of Pglet app.")]
      [string]$Name,

      [Parameter(Mandatory = $true, HelpMessage = "A handler script block for a new user session.")]
      [scriptblock]$ScriptBlock,

      [Parameter(Mandatory = $false,HelpMessage = "Makes the app available as public at pglet.io hosted service.")]
      [switch]$Public,

      [Parameter(Mandatory = $false,HelpMessage = "Makes the app available as private at pglet.io hosted service.")]
      [switch]$Private,      

      [Parameter(Mandatory = $false,HelpMessage = "Connects to the app on a self-hosted Pglet server.")]
      [string]$Server,

      [Parameter(Mandatory = $false,HelpMessage = "Authentication token for pglet.io service or a self-hosted Pglet server.")]
      [string]$Token      
    )

    $ErrorActionPreference = "Stop"

    #$Sessions = New-Object System.Collections.ArrayList
    $Sessions = [hashtable]::Synchronized(@{})

    $sessionsMonitor = {
        param($ui)

        try {
            while ($true) {
                $sids = @()
                $sids += $Sessions.Keys
                foreach($sid in $sids) {
                    $session = $Sessions[$sid]
                    if ($session.AsyncHandler.IsCompleted) {
                        try {
                            $ui.WriteLine("Session exited: " + $sid)
                            $dc = $session.PowerShell.EndInvoke($session.AsyncHandler)
                        }
                        catch {
                            $ui.WriteLine($dc.Count)
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
            $ui.WriteLine("An error occurred: " + $_.ToString())
        }
        finally {
            $ui.WriteLine("Monitor stopped")
        }
    }

    # sessions monitor
    $rsMonitor = [runspacefactory]::CreateRunspace()
    $psMonitor = [powershell]::Create()
    $psMonitor.Runspace = $rsMonitor
    $rsMonitor.Open() | Out-Null
    $rsMonitor.SessionStateProxy.SetVariable('Sessions', $Sessions)
    $psMonitor.AddScript($sessionsMonitor).AddArgument($host.UI) | Out-Null
    $psMonitor.BeginInvoke() | Out-Null

    try {
        pglet app $Name | ForEach-Object {

            $SessionID = $_
            $Runspace = [runspacefactory]::CreateRunspace()
            $PowerShell = [powershell]::Create()
            $PowerShell.Runspace = $Runspace
            $Runspace.Open() | Out-Null
            $Runspace.SessionStateProxy.SetVariable('ui', $host.UI)
            $PowerShell.AddScript("Import-Module ([IO.Path]::Combine('$PSScriptRoot', 'pglet.psm1'))") | Out-Null
            $PowerShell.AddScript('$SESSION_ID="' + $SessionID + '"') | Out-Null
            $PowerShell.AddScript($ScriptBlock).AddArgument($host.UI) | Out-Null
    
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

      [Parameter(Mandatory = $false,HelpMessage = "Makes the page available as public at pglet.io hosted service.")]
      [switch]$Public,

      [Parameter(Mandatory = $false,HelpMessage = "Makes the page available as private at pglet.io hosted service.")]
      [switch]$Private,      

      [Parameter(Mandatory = $false,HelpMessage = "Connects to the page on a self-hosted Pglet server.")]
      [string]$Server,

      [Parameter(Mandatory = $false,HelpMessage = "Authentication token for pglet.io service or a self-hosted Pglet server.")]
      [string]$Token      
    )

    $ErrorActionPreference = "Stop"

    pglet page $Name
}

function Disconnect-Pglet {
    # TODO
}

function Write-Pglet {
    [CmdletBinding()]
    param
    (
      [Parameter(Mandatory = $true, Position = 0, HelpMessage = "Pglet command to send.")]
      [string]$Command,

      [Parameter(Mandatory = $false, Position = 1, HelpMessage = "Page connection ID.")]
      [string]$Page
    )

    $ErrorActionPreference = "Stop"

    Write-Trace "Connection ID: $Page"
    Write-Trace "Command: $Command"
}

function Read-Pglet() {
    # TODO
}

function Write-Trace {
    #$ui.WriteLine("$($SESSION_ID): " + $str)
    param(
        [Parameter(Mandatory=$true, Position=0, ValueFromRemainingArguments=$true)]
        [string]$value
    )
    [System.Console]::WriteLine($value)
}

# Exported functions
Export-ModuleMember -Function Connect-PgletApp, Connect-PgletPage, Disconnect-Pglet, Write-Pglet, Read-Pglet, Write-Trace
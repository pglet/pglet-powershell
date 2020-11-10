<#

References:
https://learn-powershell.net/2013/04/19/sharing-variables-and-live-objects-between-powershell-runspaces/

#>
function Connect-PgletApp {
    <#
    .SYNOPSIS
        Creates a new Pglet app and opens a connection.

    .DESCRIPTION
        Connect-PgletApp connects to Pglet app. The app is created if not exists.

    .PARAMETER Name
        The name of Pglet app.

    .PARAMETER Handler
        A handler script block for a new user session.

    .PARAMETER Public
        Makes the app available to the public at https://app.pglet.io.

    .PARAMETER Server
        Pushes the app to a self-hosted Pglet server.

        .EXAMPLE
        Connect-PgletApp
    #>

    [CmdletBinding()]
    param
    (
      [Parameter(Mandatory=$false,HelpMessage="The name of Pglet app.")]
      [string]$Name,

      [Parameter(Mandatory=$true,HelpMessage="A handler script block for a new user session.")]
      [scriptblock]$Handler,

      [Parameter(Mandatory=$false,HelpMessage="Makes the app available to the public at https://app.pglet.io.")]
      [switch]$Public,

      [Parameter(Mandatory=$false,HelpMessage="Pushes the app to a self-hosted Pglet server.")]
      [string]$Server      
    )

    $ErrorActionPreference = "Stop"

    #$Sessions = New-Object System.Collections.ArrayList
    $Sessions = [hashtable]::Synchronized(@{})

    $sessionsMonitor = {
        param($h)

        try {
            #$h.UI.WriteLine('aaa')
            #$h.UI.WriteLine($Sessions)

            while ($true) {
                $sids = @()
                $sids += $Sessions.Keys
                foreach($sid in $sids) {
                    $session = $Sessions[$sid]
                    if ($session.AsyncHandler.IsCompleted) {
                        $h.UI.WriteLine("Session exited: " + $sid)
                        $session.Runspace.Close()
                        $session.PowerShell.Dispose()
                        $Sessions.Remove($sid)
                    }
                }
                Start-Sleep -s 2
            }
        }
        catch {
            $h.UI.WriteLine("An error occurred: " + $_.ToString())
        }
        finally {
            $h.UI.WriteLine("Monitor stopped")
        }
    }

    # sessions monitor
    $rsMonitor = [runspacefactory]::CreateRunspace()
    $psMonitor = [powershell]::Create()
    $psMonitor.Runspace = $rsMonitor
    $rsMonitor.Open() | Out-Null
    $rsMonitor.SessionStateProxy.SetVariable('Sessions', $Sessions)
    $psMonitor.AddScript($sessionsMonitor).AddArgument($host) | Out-Null
    $psMonitor.BeginInvoke()

    try {
        pglet app $Name | ForEach-Object {
            $SessionID = $_
            $Runspace = [runspacefactory]::CreateRunspace()
            $PowerShell = [powershell]::Create()
            $PowerShell.Runspace = $Runspace
            $Runspace.Open() | Out-Null
            $PowerShell.AddScript($Handler).AddArgument($host).AddArgument($SessionID) | Out-Null
    
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
        $rsMonitor.Close()
        $psMonitor.Dispose()
    }
}
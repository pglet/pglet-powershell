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

    if ($IsLinux -or $IsMacOS) {
        $pargs += "--uds"
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
        pglet $pargs | ForEach-Object {

            if ($_ -match "(?<session_id>[^\s]+)\s(?<page_url>[^\s]+)") {
                $sessionID = $Matches["session_id"]
                #$PageURL = $Matches["page_url"]
            } else {
                throw "Invalid pglet results: $_"
            }

            $Runspace = [runspacefactory]::CreateRunspace()
            $PowerShell = [powershell]::Create()
            $PowerShell.Runspace = $Runspace
            $Runspace.Open() | Out-Null
            $PowerShell.AddScript("Import-Module ([IO.Path]::Combine('$PSScriptRoot', 'pglet.psm1'))") | Out-Null
            $PowerShell.AddScript('$global:PGLET_CONNECTION_ID="' + $sessionID + '"') | Out-Null
            $PowerShell.AddScript('$global:PGLET_CONNECTIONS=[hashtable]::Synchronized(@{})') | Out-Null
            $PowerShell.AddScript($ScriptBlock) | Out-Null
    
            # add session to monitor
            $Sessions[$sessionID] = @{
                SessionID = $sessionID
                PowerShell = $PowerShell
                Runspace = $Runspace
                AsyncHandler = $PowerShell.BeginInvoke()
            }

            Write-Trace "Session started: $sessionID"
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

    if ($IsLinux -or $IsMacOS) {
        $pargs += "--uds"
    }

    # run pglet client and get results
    $presults = (pglet $pargs)


    if ($presults -match "(?<pipe_id>[^\s]+)\s(?<page_url>[^\s]+)") {
        $pipeId = $Matches["pipe_id"]
        $PageURL = $Matches["page_url"]
    } else {
        throw "Invalid pglet results: $presults"
    }

    $global:PGLET_CONNECTION_ID = $pipeId

    Write-Host "Page URL: $PageURL"

    return $pipeId
}

function openConnection($pipeId) {

    $conn = $PGLET_CONNECTIONS[$pipeId]
    if ($conn) {
        return $conn
    }

    # establish connection
    $conn = @{
        pipe = new-object System.IO.Pipes.NamedPipeClientStream($pipeId)
        eventPipe = new-object System.IO.Pipes.NamedPipeClientStream("$pipeId.events")
    }

    # connect pipes
    $conn.pipe.Connect(5000)
    $conn.eventPipe.Connect(5000)

    # create readers and writers
    $conn.pipeReader = new-object System.IO.StreamReader($conn.pipe)
    $conn.pipeWriter = new-object System.IO.StreamWriter($conn.pipe)
    $conn.eventPipeReader = new-object System.IO.StreamReader($conn.eventPipe)

    $global:PGLET_CONNECTIONS.Add($pipeId, $conn)

    return $conn
}

function Disconnect-Pglet {
    [CmdletBinding()]
    param
    (
        [Parameter(Mandatory = $false, Position = 0, HelpMessage = "Page connection ID.")]
        [string]$Page
    )

    $ErrorActionPreference = "Stop"

    $pipeId = $Page

    if (-not $pipeId) {
        $pipeId = $PGLET_CONNECTION_ID
    }

    if (-not $pipeId) {
        throw "No active connections."
    }

    $conn = $PGLET_CONNECTIONS[$pipeId]
    if ($conn) {
        $conn.pipe.Close()
        $conn.eventPipe.Close()
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

    $pipeId = $Page

    if (-not $pipeId) {
        $pipeId = $PGLET_CONNECTION_ID
    }

    if (-not $pipeId) {
        throw "No active connections."
    }

    $conn = openConnection $pipeId

    # send command
    $conn.pipeWriter.WriteLine($Command)
    $conn.pipeWriter.Flush()

    # get results
    $result = $conn.pipeReader.ReadLine()

    # parse results
    $OK_RESULT = "ok"
    $ERROR_RESULT = "error"
    
    #Write-Host "Result: $result"

    if ($result -eq $OK_RESULT) {
        return ""
    } elseif ($result.StartsWith("$OK_RESULT ")) {
        return $result.Substring($OK_RESULT.Length + 1)
    } elseif ($result.StartsWith("$ERROR_RESULT ")) {
        throw $result.Substring($ERROR_RESULT.Length + 1)
    } else {
        throw "Unexpected result: $result"
    }
}

function Wait-PgletEvent() {
    [CmdletBinding()]
    param
    (
        [Parameter(Mandatory = $false, Position = 0, HelpMessage = "Page connection ID.")]
        [string]$Page
    )

    $ErrorActionPreference = "Stop"

    $pipeId = $Page

    if (-not $pipeId) {
        $pipeId = $PGLET_CONNECTION_ID
    }

    if (-not $pipeId) {
        throw "No active connections."
    }

    $conn = openConnection $pipeId  

    $line = $conn.eventPipeReader.ReadLine()
    #Write-Host "Event: $line"
    if ($line -match "(?<target>[^\s]+)\s(?<name>[^\s]+)(\s(?<data>.+))*") {
        return @{
            Target = $Matches["target"]
            Name = $Matches["name"]
            Data = $Matches["data"]
        }
    } else {
        throw "Invalid event data: $line"
    }
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
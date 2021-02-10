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

$global:PGLET_EXE = ""
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

      [Parameter(Mandatory = $false, HelpMessage = "Makes the app available as public at pglet.io service or a self-hosted Pglet server")]
      [switch]$Web,

      [Parameter(Mandatory = $false, HelpMessage = "Connects to the app on a self-hosted Pglet server.")]
      [string]$Server,

      [Parameter(Mandatory = $false, HelpMessage = "Authentication token for pglet.io service or a self-hosted Pglet server.")]
      [string]$Token,

      [Parameter(Mandatory = $false, HelpMessage = "Do not open browser window")]
      [switch]$NoWindow,

      [Parameter(Mandatory = $false, HelpMessage = "Interval in milliseconds between 'tick' events; disabled if not specified.")]
      [int]$Ticker = $null
    )

    $ErrorActionPreference = "Stop"

    $pargs = @()
    $pargs += "app"
    if ($Name) {
        $pargs += $Name
    }

    if ($Web.IsPresent) {
        $pargs += "--web"
    }

    if ($NoWindow.IsPresent) {
        $pargs += "--no-window"
    }

    if ($Ticker) {
        $pargs += "--ticker"
        $pargs += $Ticker
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
                            $session.PowerShell.EndInvoke($session.AsyncHandler)
                        }
                        catch {
                            Write-Trace "Error terminating session: $_"
                        }
                        $session.Runspace.Close()
                        $session.PowerShell.Dispose()
                        $Sessions.Remove($sid)
                    }
                }
                Start-Sleep -s 1
            }
        }
        catch {
            Write-Trace "An error occurred in session monitor: $_"
        }
        finally {
            Write-Trace "Sessions monitor stopped"
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
        & $PGLET_EXE $pargs | ForEach-Object {

            if (-not $PageURL) {
                $PageURL = $_
                Write-Host "Page URL: $PageURL"
                return
            }

            $sessionID = $_

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
        Write-Host "Terminating app..."

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

      [Parameter(Mandatory = $false, HelpMessage = "Makes the page available as public at pglet.io service or a self-hosted Pglet server")]
      [switch]$Web,  

      [Parameter(Mandatory = $false, HelpMessage = "Connects to the page on a self-hosted Pglet server.")]
      [string]$Server,

      [Parameter(Mandatory = $false, HelpMessage = "Authentication token for pglet.io service or a self-hosted Pglet server.")]
      [string]$Token,

      [Parameter(Mandatory = $false, HelpMessage = "Do not open browser window")]
      [switch]$NoWindow,

      [Parameter(Mandatory = $false, HelpMessage = "Interval in milliseconds between 'tick' events; disabled if not specified.")]
      [int]$Ticker = $null
    )

    $ErrorActionPreference = "Stop"

    $pargs = @()
    $pargs += "page"
    if ($Name) {
        $pargs += $Name
    }

    if ($Web.IsPresent) {
        $pargs += "--web"
    }

    if ($NoWindow.IsPresent) {
        $pargs += "--no-window"
    }

    if ($Ticker) {
        $pargs += "--ticker"
        $pargs += $Ticker
    }    

    if ($Server) {
        $pargs += "--server"
        $pargs += $Server
    }

    if ($Token) {
        $pargs += "--token"
        $pargs += $Token
    }

    # run pglet client and get results
    $presults = (& $PGLET_EXE $pargs)

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

    if (-not $IsLinux -and -not $IsMacOS) {
        # use named pipes on Windows
        $conn = @{
            pipe = new-object System.IO.Pipes.NamedPipeClientStream($pipeId)
            eventPipe = new-object System.IO.Pipes.NamedPipeClientStream("$pipeId.events")
        }

        # connect pipes
        $conn.pipe.Connect(5000)
        $conn.eventPipe.Connect(5000)

        # create readers and writers
        $conn.pipeReader = new-object System.IO.StreamReader($conn.pipe)
        $utf8 = new-object System.Text.UTF8Encoding($false, $true)
        $conn.pipeWriter = new-object System.IO.StreamWriter($conn.pipe, $utf8, 65535)
        $conn.pipeWriter.AutoFlush = $true
        $conn.eventPipeReader = new-object System.IO.StreamReader($conn.eventPipe)
    } else {
        $conn = @{
            pipeName = $pipeId
        }
    }

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
    if ($conn -and (-not $IsLinux -and -not $IsMacOS)) {
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

    $waitResult = $true
    if ($Command -match "(?<commandName>[^\s]+)\s(.*)") {
        $commandName = $Matches["commandName"]
        #Write-Host "COMMAND: $commandName"
        if ($commandName.toLower().endsWith("f")) {
            $waitResult = $false
        }
    }

    if ($IsLinux -or $IsMacOS) {
        $conn.pipeWriter = New-Object System.IO.StreamWriter($conn.pipeName)
    }

    # send command
    $conn.pipeWriter.WriteLine($command)

    if ($IsLinux -or $IsMacOS) {
        $conn.pipeWriter.Close()
    }

    if ($waitResult) {
        # parse results
        $ERROR_RESULT = "error"

        try {
            if ($IsLinux -or $IsMacOS) {
                $conn.pipeReader = New-Object System.IO.StreamReader($conn.pipeName)
            }
            
            $result = $conn.pipeReader.ReadLine()
        
            if ($result.StartsWith("$ERROR_RESULT ")) {
                throw $result.Substring($ERROR_RESULT.Length + 1)
            } elseif ($result -match "(?<lines_count>[\d]+)\s(?<result>.*)") {
                $lines_count = [int]$Matches["lines_count"]
                $result = $Matches["result"]
        
                # read the rest of multi-line result
                for($i = 0; $i -lt $lines_count; $i++) {
                    $line = $conn.pipeReader.ReadLine()
                    $result = "$result`n$line"
                }
            } else {
                throw "Invalid result: $result"
            }
            return $result
        } finally {
            if ($IsLinux -or $IsMacOS) {
                $conn.pipeReader.Close()
            }
        }
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

    if ($IsLinux -or $IsMacOS) {
        $conn.eventPipeReader = New-Object System.IO.StreamReader("$($conn.pipeName).events")
    }
    $line = $conn.eventPipeReader.ReadLine()
    if ($IsLinux -or $IsMacOS) {
        $conn.eventPipeReader.Close()
    }

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

function installPglet {
    [CmdletBinding()]
    Param()

    $ErrorActionPreference = "Stop"

    $global:PGLET_EXE = "pglet.exe"
    if ($IsLinux -or $IsMacOS) {
        $global:PGLET_EXE = "pglet"
    }

    # check if pglet.exe is in PATH (dev mode)
    $pgletInPath = Get-Command $global:PGLET_EXE -ErrorAction SilentlyContinue
    if ($pgletInPath) {
        Write-Verbose "Pglet in PATH found: $($pgletInPath.Path)"
        $global:PGLET_EXE = $pgletInPath.Path
        return
    }

    $pgletHome = [IO.Path]::Combine($HOME, ".pglet")
    $pgletBin = [IO.Path]::Combine($pgletHome, "bin")
    $global:PGLET_EXE = [IO.Path]::Combine($pgletBin, $global:PGLET_EXE)

    # create bin dir
    if (-not (Test-Path $pgletBin)) {
        Write-Verbose "Creating $pgletBin directory"
        New-Item -ItemType Directory -Path $pgletBin -Force | Out-Null
    }

    # Min version required by PS module
    $ver = $MyInvocation.MyCommand.Module.PrivateData.Pglet.MinimumVersion

    # target
    $fileName = "pglet-$ver-windows-amd64.zip"
    if ($IsLinux) {
        $fileName = "pglet-$ver-linux-amd64.tar.gz"
    } elseif ($IsMacOS) {
        $fileName = "pglet-$ver-darwin-amd64.tar.gz"
    }

    # GitHub requires TLS 1.2
    [Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12

    # Installed version
    if (Test-Path $PGLET_EXE) {
        try {
            $installedVer = (& $PGLET_EXE --version)
        } catch {}        
    }

    if ($installedVer -and ($installedVer -eq $ver)) {
        Write-Verbose "Newer version is already installed"
        return
    }

    Write-Host "Installing Pglet v$ver..." -NoNewline
    $pgletUri = "https://github.com/pglet/pglet/releases/download/v$ver/$fileName"
    $packagePath = [IO.Path]::Combine($pgletHome, $fileName)
    (New-Object Net.WebClient).DownloadFile($pgletUri, $packagePath)

    Write-Verbose "Unzipping..."
    if ($IsLinux -or $IsMacOS) {
        # untar
        tar zxf $packagePath -C $pgletBin
    } else {
        # unzip
        Expand-Archive -Path $packagePath -DestinationPath $pgletBin -Force
    }
    Remove-Item $packagePath -Force

    $installedVer = (& $PGLET_EXE --version)
    Write-Host "OK"
}

installPglet -verbose

New-Alias -Name ipg -Value Invoke-Pglet

# Exported functions
Export-ModuleMember -Function Connect-PgletApp, Connect-PgletPage, Disconnect-Pglet, Invoke-Pglet, Wait-PgletEvent, Write-Trace -Alias ipg
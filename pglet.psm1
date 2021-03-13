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


function Write-Trace {
    param(
        [Parameter(Mandatory=$true, Position=0, ValueFromRemainingArguments=$true)]
        [string]$value
    )
    [System.Console]::WriteLine($value)
}

New-Alias -Name ipg -Value Invoke-Pglet

# Exported functions
#Export-ModuleMember -Function Disconnect-Pglet, Write-Trace -Alias ipg
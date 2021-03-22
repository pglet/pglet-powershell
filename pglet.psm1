function Write-Trace {
    param(
        [Parameter(Mandatory=$true, Position=0, ValueFromRemainingArguments=$true)]
        [string]$value
    )
    [System.Console]::WriteLine($value)
}

New-Alias -Name Button -Value New-PgletButton
New-Alias -Name Text -Value New-PgletText
New-Alias -Name Textbox -Value New-PgletTextbox
New-Alias -Name Stack -Value New-PgletStack
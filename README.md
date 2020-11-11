# PowerShell bindings for Pglet

## Requirements

* Windows PowerShell 5.1
* PowerShell Core 7 on Windows, Linux or macOS

## Installation

Install `pglet` module from PowerShell Gallery:

    Install-Module pglet

## Getting started

Create a new `hello.ps1` with the following content:

```posh
Import-Module pglet
Connect-Pglet myapp -App -Handler {
    Invoke-Pglet "add text value='Hello, world!'"
    Disconnect-Pglet
}
```
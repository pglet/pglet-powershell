# Pglet client for PowerShell

[Pglet](https://pglet.io) (*"piglet"*) is a rich user interface (UI) framework for scripts written in PowerShell or any other language. 
Pglet renders web UI, so you can easily [build web apps](https://pglet.io/docs/quickstart) with PowerShell.
Knowledge of HTML/CSS/JavaScript is not required as you build UI with [controls](https://pglet.io/docs/reference/controls). Pglet controls are built with [Fluent UI React](https://developer.microsoft.com/en-us/fluentui#/controls/web) to ensure your programs look cool and professional.

## Requirements

* Windows PowerShell 5.1
* PowerShell Core 7 on Windows, Linux or macOS

## Installation

Install `pglet` module from PowerShell Gallery:

    Install-Module pglet

## Hello, world!

Create a new `hello.ps1` with the following content:

```posh
Import-Module pglet
Connect-PgletPage
Invoke-Pglet "add text value='Hello, world!'"
```

Run `hello.ps1` in your PowerShell session and in a new browser window you'll get:

![Sample app in a browser](https://pglet.io/img/docs/quickstart-hello-world.png "Sample app in a browser")

Here is a local page served by an instance of Pglet server started in the background on your computer.

## Make it web

Add `-Web` parameter to `Connect-PgletPage` call:

```posh
Connect-PgletPage -Web
Invoke-Pglet "add text value='Hello, world!'"
```

This time page will be created on [Pglet hosted service](https://pglet.io/docs/pglet-service).

Read [PowerShell tutorial](https://pglet.io/docs/tutorials/powershell) for further information and more examples.
Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

$page = Connect-PgletPage -Name "text-1" -NoWindow

try {
    $page.Clean()

    $t = Text -Markdown -Value '# Using Markdown with Pglet

You can add `-Markdown` parameter to `Text` cmdlet
to output **rich** *text*.

[GitHaub Flavored Markdown](https://github.github.com/gfm/) is supported.

This is a code snippet:

```
import Pglet
```
    '
    $page.Add($t)

} finally {
    $page.Close()
}
Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

Connect-PgletPage -Name "index" -NoWindow

Invoke-Pglet "clean page"
Invoke-Pglet "set page verticalFill=true horizontalAlign=center verticalAlign=center"

Invoke-Pglet "add
stack width='50%' height='30%' scrolly
    text value='Code sample' size=xlarge
    text markdown value='``````powershell\nInvoke-Pglet `"clean page`" Invoke-Pglet `"clean page`" Invoke-Pglet `"clean page`" Invoke-Pglet `"clean page`" Invoke-Pglet `"clean page`" Invoke-Pglet `"clean page`" Invoke-Pglet `"clean page`" Invoke-Pglet `"clean page`"\n``````'
    text markdown value='``````powershell\nInvoke-Pglet `"clean page`"\n``````'
    text pre size=large value='`$i = 0\nWrite-Host `"Test!`"'
    text pre value='Another pre-formatted text of a regular size. Another pre-formatted text of a regular size. Another pre-formatted text of a regular size'
    text value='Another regular text of a regular size. Another regular text of a regular size. Another regular text of a regular size'
    text value='`"line 1`"'
    text value='\'line 2\''
    text value=`"'line 3'`"
    text value=`"\`"line\n4\`"`"
    text value='C:\\some path\\aaa'
    text value=`"C:\\some path with\\'quo\ntes'\\aaa`"
    text value=`"C:\\another path\\Check't\\aaa`"
    text value='C:\\another path\\Check\'t\\aaa'
"
Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

Connect-PgletPage -Name "index" -NoWindow

Invoke-Pglet "clean page"

Invoke-Pglet "add
text value='Code sample' size=xlarge
text markdown value='``````powershell\nInvoke-Pglet `"clean page`"\n``````'
text pre size=large value='`$i = 0\nWrite-Host `"Test!`"'
text value='`"line 1`"'
text value='\'line 2\''
text value=`"'line 3'`"
text value=`"\`"line\n4\`"`"
text value='C:\\some path\\aaa'
text value=`"C:\\some path with\\'quo\ntes'\\aaa`"
text value=`"C:\\another path\\Check't\\aaa`"
text value='C:\\another path\\Check\'t\\aaa'
"
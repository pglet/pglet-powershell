Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

$page = Connect-PgletPage "bar-chart"
$page.clean($true)

$chart = BarChart -Points @(
    BarChartDataPoint -X 10 -Y 20 -Legend "Disk A:"
    BarChartDataPoint -X 20 -Y 100 -Legend "Disk B:"
) -DataMode Percentage

$page.add($chart)

$p = BarChartDataPoint -X 0 -Y 30
$chart.points.add($p)
$page.update()

for($i = 0; $i -le 30; $i++) {
    $p.x = $i
    $page.update()
    Start-Sleep -ms 100
}
Remove-Module pglet -ErrorAction SilentlyContinue
Import-Module ([IO.Path]::Combine((get-item $PSScriptRoot).parent.FullName, 'pglet.psd1'))

Connect-PgletPage -Name "index" -NoWindow

Invoke-Pglet "clean page"

Invoke-Pglet "add
text value='No image fit' size=xlarge
text size=medium value='With no imageFit property set, the width and height props control the size of the frame. Depending on which of those props is used, the image may scale to fit the frame.'

text size=small value='Without a width or height specified, the frame remains at its natural size and the image will not be scaled.'
image src='http://placehold.it/350x150' title='sample image' alt='Example with no image fit value and no height or width is specified.'
image width=600 src='http://placehold.it/350x150' alt='Example with no image fit value and only width is specified.'
image height=100 src='http://placehold.it/350x150' alt='Example with no image fit value and only height is specified.'
image width=100 height=100 src='http://placehold.it/350x150' alt='Example with no image fit value and height or width is specified'

text value='fit: center' size=xlarge
image fit=center width=350 height=150 src='http://placehold.it/800x300' alt='Example of the image fit value `"center`" on an image larger than the frame.'
"
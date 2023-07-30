$buildFolder = (Get-Item -Path "./" -Verbose).FullName

$outputFolder = Join-Path $buildFolder "Releases/"
$devFolder = Join-Path $buildFolder "src/DevFolder/"

Set-Location $devFolder
dotnet publish --output (Join-Path $outputFolder "DevFolder/")
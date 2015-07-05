$root = (split-path -parent $MyInvocation.MyCommand.Definition)
$lib = "$root\.package\lib\45\"
If (Test-Path $lib)
{
	Remove-Item $lib -recurse
}
new-item -Path $lib -ItemType directory
new-item -Path $root\.nupkg -ItemType directory -force
Copy-Item $root\Lookum.Framework\bin\Debug\* $lib

$version = [System.Diagnostics.FileVersionInfo]::GetVersionInfo("$lib\Lookum.Framework.dll").ProductVersion

Write-Host "Setting .nuspec version tag to $version"

$content = (Get-Content $root\Lookum.nuspec -Encoding UTF8) 
$content = $content -replace '\$version\$',$version

$content | Out-File $root\.package\Lookum.compiled.nuspec -Encoding UTF8

& $root\.nuget\NuGet.exe pack $root\.package\Lookum.compiled.nuspec -OutputDirectory $root\.nupkg
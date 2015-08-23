$msbuild = "C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe"
$nuget = ".\nuget.exe"

$target = "packages"

function Package($project)
{
	# 1. Build project
    & $msbuild $project /t:rebuild /p:Configuration=Release
	
	if($LASTEXITCODE -eq 0) 
	{ 
		# 2. Create package
		& $nuget pack $project -Prop Configuration=Release
	}
	else
	{
		Write-Host "[$project] Failed to build" -ForegroundColor Red
	}
	
	# 5. Success
	Write-Host "[$project] Packaging Successful" -ForegroundColor Green
}

# 1. Delete old packages

if(Test-Path -Path $target)
{
	Remove-Item $target -Force -Recurse
}

# 2. Package

Package "..\src\SimpleDAO\SimpleDAO.csproj"
Package "..\src\SimpleDAO.EntityFramework\SimpleDAO.EntityFramework.csproj"

# 3. Move package

if(!(Test-Path -Path $target))
{
	New-Item -ItemType Directory -Path $target
}
Move-Item -Path "*.nupkg" -Destination $target

# 4. Pause

Write-Host -NoNewLine 'Press any key to continue...';
$null = $Host.UI.RawUI.ReadKey('NoEcho,IncludeKeyDown');
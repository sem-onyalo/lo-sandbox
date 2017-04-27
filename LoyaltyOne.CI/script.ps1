param(
	[string]$workspacePath = "..",
	[string]$deployOnBuild = "false",
	[string]$deployProfile = "dev-local",
	[string]$vsVersion = "14.0"
)

Import-Module WebAdministration

$buildCmd = "msbuild"

$config = (get-content -Raw "$PSScriptRoot\config.json") | ConvertFrom-Json

foreach($component in $config.components)
{
	if ($component.projectType -eq 'test')
	{
		$buildSourcePath = ("{0}\{1}" -f  $workspacePath, $component.projectPath) 

		& $buildCmd $buildSourcePath "/t:RunTests" ("/property:BuildProjectReferences=true;VisualStudioVersion={0}" -f $vsVersion)

		if ($LASTEXITCODE -ne 0)
		{
			Write-Host "Aborting build, test(s) failed for" $component.projectPath
			break
		}
	}
	elseif ($component.projectType -eq 'site')
	{
		Write-Host "Restarting application pool for " $component.siteName
		$site = $component.siteName
		$appPool = (Get-Item "IIS:\Sites\$site"| Select-Object applicationPool).applicationPool
		Restart-WebAppPool $appPool
	}
	else
	{
		$buildSourcePath = ("{0}\{1}" -f  $workspacePath, $component.projectPath) 

		& $buildCmd $buildSourcePath "/t:Rebuild" ("/property:BuildProjectReferences=true;DeployOnBuild={0};PublishProfile={1};VisualStudioVersion={2}" -f $deployOnBuild, $deployProfile, $vsVersion)
	}
}

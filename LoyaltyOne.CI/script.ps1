param(
	[string]$workspacePath = "..",
	[string]$deployOnBuild = "false",
	[string]$deployProfile = "dev-local",
	[string]$vsVersion = "14.0"
)

Import-Module WebAdministration

$buildCmd = "msbuild"
$buildCmdTargets = "/t:Rebuild"
$buildCmdProperties = ("/property:BuildProjectReferences=true;DeployOnBuild={0};PublishProfile={1};VisualStudioVersion={2}" -f $deployOnBuild, $deployProfile, $vsVersion)

$config = (get-content -Raw "$PSScriptRoot\config.json") | ConvertFrom-Json

foreach($component in $config.components)
{
	if ($component.projectType -eq 'site')
	{
		Write-Host "Restarting application pool for " $component.siteName
		$site = $component.siteName
		$appPool = (Get-Item "IIS:\Sites\$site"| Select-Object applicationPool).applicationPool
		Restart-WebAppPool $appPool
	}

	$buildSourcePath = ("{0}\{1}" -f  $workspacePath, $component.projectPath) 

	& $buildCmd $buildSourcePath $buildCmdTargets $buildCmdProperties
}

param(
	[string]$workspacePath = "..",
	[string]$deployOnBuild = "false",
	[string]$deployProfile = "dev-local",
	[string]$vsVersion = "14.0"
)

$buildCmd = "msbuild"
$buildCmdTargets = "/t:Rebuild"
$buildCmdProperties = ("/property:BuildProjectReferences=true;DeployOnBuild={0};PublishProfile={1};VisualStudioVersion={2}" -f $deployOnBuild, $deployProfile, $vsVersion)

$config = (get-content -Raw "$PSScriptRoot\config.json") | ConvertFrom-Json

foreach($component in $config.components)
{
	$buildSourcePath = ("{0}\{1}" -f  $workspacePath, $component.projectPath) 

	& $buildCmd $buildSourcePath $buildCmdTargets $buildCmdProperties
}

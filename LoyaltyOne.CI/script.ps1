param(
	[string]$workspacePath = "..",
	[string]$deployOnBuild = "false",
	[string]$deployProfile = "dev-local",
	[string]$vsVersion = "14.0"
)

$buildCmd = "msbuild"
$buildSourcePath = ("{0}\{1}" -f  $workspacePath, "LoyaltyOne\LoyaltyOne.Web\LoyaltyOne.Web.csproj") 
$buildCmdProperties = ("/property:DeployOnBuild={0};PublishProfile={1};VisualStudioVersion={2}" -f $deployOnBuild, $deployProfile, $vsVersion)

& $buildCmd $buildSourcePath $buildCmdProperties

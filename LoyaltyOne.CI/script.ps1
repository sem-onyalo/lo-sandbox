$buildCmd = "msbuild"
$buildSourcePath = "..\LoyaltyOne\LoyaltyOne.Web\LoyaltyOne.Web.csproj"
$buildCmdProperties = ("/property:DeployOnBuild={0};PublishProfile={1};VisualStudioVersion={2}" -f "true", "dev-local", "14.0")

& $buildCmd $buildSourcePath $buildCmdProperties

const spawn = require('child_process').spawnSync;

var nugetPath = process.env.NUGET_PACKAGES;
if (!nugetPath) {
	nugetPath=process.env.HOME+"/.nuget/packages";
}

var refitGenerator=nugetPath+"/refit/"+process.argv[2]+"/tools/InterfaceStubGenerator.exe";
var folder=process.argv[3];
var result;
if (process.platform == "win32") {
	result = spawn(refitGenerator, [folder+"/RefitStubs.g.cs", folder], { stdio: 'inherit' });
}
else {
	result = spawn("mono", [refitGenerator, folder+"/RefitStubs.g.cs", folder], { stdio: 'inherit' });
}
process.exit(result.status);
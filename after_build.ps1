Write-Host "Copying appsettings.json into build directory"

$strConfigSource = "$env:APPVEYOR_BUILD_FOLDER\Shared.Tests\appsettings.json"
$strConfigDest = "$env:APPVEYOR_BUILD_FOLDER\Cloudinary.Test\bin\Release\net40\appsettings.json"

Copy-Item $strConfigSource -Destination $strConfigDest


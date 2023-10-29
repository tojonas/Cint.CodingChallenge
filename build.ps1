
dotnet restore Cint.CodingChallenge.sln -v q
if(!$LASTEXITCODE) { dotnet clean Cint.CodingChallenge.sln -v q }
if(!$LASTEXITCODE) { dotnet build Cint.CodingChallenge.sln -v q }
if(!$LASTEXITCODE) { dotnet test --no-build test\Cint.CodingChallenge.Web.Test\Cint.CodingChallenge.Web.Test.csproj }

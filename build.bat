IF NOT EXIST paket.lock (
    START /WAIT .paket/paket.exe install
)
dotnet restore src/HelloGiraffe
dotnet build src/HelloGiraffe

dotnet restore tests/HelloGiraffe.Tests
dotnet build tests/HelloGiraffe.Tests
dotnet test tests/HelloGiraffe.Tests

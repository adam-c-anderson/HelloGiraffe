if [ ! -e "paket.lock" ]
then
    exec mono .paket/paket.exe install
fi
dotnet restore src/HelloGiraffe
dotnet build src/HelloGiraffe

dotnet restore tests/HelloGiraffe.Tests
dotnet build tests/HelloGiraffe.Tests
dotnet test tests/HelloGiraffe.Tests

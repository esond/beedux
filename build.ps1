dotnet build --configuration Release
nuget delete Beedux 0.1.0 -source $HOME\.nuget\packages
nuget add .\src\bin\Release\Beedux.0.1.0.nupkg -source $HOME\.nuget\packages
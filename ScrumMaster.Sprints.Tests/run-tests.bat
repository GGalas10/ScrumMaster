@echo off
echo Running tests with Code Coverage...
dotnet test --collect:"XPlat Code Coverage" --results-directory:"TestResults/"
reportgenerator -reports:TestResults/**/coverage.cobertura.xml -targetdir:coveragereport
start coveragereport\index.html
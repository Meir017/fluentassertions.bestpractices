param (
    [switch]$FormatAndExecuteTestsAgain
)

Push-Location src
Push-Location FluentAssertions.Analyzers.FluentAssertionAnalyzerDocs
dotnet test
Pop-Location
Pop-Location

if ($FormatAndExecuteTestsAgain) {
    Push-Location src
    Push-Location FluentAssertions.Analyzers.FluentAssertionAnalyzerDocs
    dotnet format analyzers FluentAssertions.Analyzers.FluentAssertionAnalyzerDocs.csproj --diagnostics FAA0001 FAA0003 FAA0004 --severity info --verbosity diagnostic
    Pop-Location
    Pop-Location

    Push-Location src
    Push-Location FluentAssertions.Analyzers.FluentAssertionAnalyzerDocsGenerator
    dotnet run verify
    Pop-Location
    Pop-Location
}

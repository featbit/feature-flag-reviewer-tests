[CmdletBinding()]
param(
    [Parameter()]
    [string]$ReviewerRoot = (Join-Path $PSScriptRoot '..\..\featbit-demo\feature-flag-reviewer'),

    [Parameter()]
    [ValidateSet('V1', 'V2')]
    [string]$BaseVersion = 'V1',

    [Parameter(Mandatory)]
    [string]$Output,

    [Parameter()]
    [string]$ReviewOutput
)

$ErrorActionPreference = 'Stop'
$repositoryRoot = [System.IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..'))
$reviewerRoot = [System.IO.Path]::GetFullPath($ReviewerRoot)
$reviewerSolution = Join-Path $reviewerRoot 'FeatureFlagReviewer.slnx'
$baseSolutionName = if ($BaseVersion -eq 'V2') { 'SnapshotBaseV2.slnx' } else { 'SnapshotBase.slnx' }
$baseSolution = Join-Path $repositoryRoot $baseSolutionName
$outputPath = [System.IO.Path]::GetFullPath($Output, (Get-Location).Path)
$outputDirectory = [System.IO.Path]::GetDirectoryName($outputPath)
$reviewOutputPath = if ([string]::IsNullOrWhiteSpace($ReviewOutput)) {
    [System.IO.Path]::ChangeExtension($outputPath, '.md')
} else {
    [System.IO.Path]::GetFullPath($ReviewOutput, (Get-Location).Path)
}
$reviewOutputDirectory = [System.IO.Path]::GetDirectoryName($reviewOutputPath)

if (-not (Test-Path -LiteralPath $reviewerSolution -PathType Leaf)) {
    throw "SnapshotScanner project root is invalid: $reviewerRoot"
}

if (Test-Path -LiteralPath $outputPath) {
    throw "Refusing to overwrite an existing report: $outputPath"
}

if (Test-Path -LiteralPath $reviewOutputPath) {
    throw "Refusing to overwrite an existing human review report: $reviewOutputPath"
}

if (-not (Test-Path -LiteralPath $outputDirectory -PathType Container)) {
    New-Item -ItemType Directory -Path $outputDirectory | Out-Null
}


if (-not (Test-Path -LiteralPath $reviewOutputDirectory -PathType Container)) {
    New-Item -ItemType Directory -Path $reviewOutputDirectory | Out-Null
}

dotnet restore $reviewerSolution --nologo
if ($LASTEXITCODE -ne 0) {
    throw "SnapshotScanner restore failed with exit code $LASTEXITCODE."
}

dotnet build $reviewerSolution --no-restore --nologo
if ($LASTEXITCODE -ne 0) {
    throw "SnapshotScanner build failed with exit code $LASTEXITCODE."
}

dotnet restore $baseSolution --nologo
if ($LASTEXITCODE -ne 0) {
    throw "Snapshot Base restore failed with exit code $LASTEXITCODE."
}

dotnet build $baseSolution --no-restore --nologo
if ($LASTEXITCODE -ne 0) {
    throw "Snapshot Base build failed with exit code $LASTEXITCODE."
}

$cliPath = Join-Path $reviewerRoot 'src\FeatBit.FeatureFlagReviewer.Cli\bin\Debug\net10.0\feature-flag-reviewer.dll'
if (-not (Test-Path -LiteralPath $cliPath -PathType Leaf)) {
    throw "Compiled SnapshotScanner CLI was not found: $cliPath"
}

Write-Host "Scanner binary: $cliPath"
Write-Host "Scanned solution: $baseSolution"
dotnet $cliPath scan --path $baseSolution --output $outputPath
if ($LASTEXITCODE -ne 0) {
    throw "Snapshot scan failed with exit code $LASTEXITCODE."
}


dotnet $cliPath render --report $outputPath --workspace $baseSolution --output $reviewOutputPath
if ($LASTEXITCODE -ne 0) {
    throw "Snapshot review render failed with exit code $LASTEXITCODE."
}

$hash = (Get-FileHash -LiteralPath $outputPath -Algorithm SHA256).Hash
$reviewHash = (Get-FileHash -LiteralPath $reviewOutputPath -Algorithm SHA256).Hash
Write-Host "Report: $outputPath"
Write-Host "SHA-256: $hash"
Write-Host "Human review: $reviewOutputPath"
Write-Host "Human review SHA-256: $reviewHash"
Write-Host 'No accuracy verdict was produced. Read the Markdown first, then inspect the JSON for machine details.'

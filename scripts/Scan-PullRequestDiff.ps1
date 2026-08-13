[CmdletBinding()]
param(
    [Parameter()]
    [string]$ReviewerRoot = (Join-Path $PSScriptRoot '..\..\featbit-demo\feature-flag-reviewer'),

    [Parameter()]
    [string]$BaseReport = (Join-Path $PSScriptRoot '..\reports\snapshot-base-v4.json'),

    [Parameter(Mandatory)]
    [string]$HeadOutput,

    [Parameter(Mandatory)]
    [string]$HeadReviewOutput,

    [Parameter(Mandatory)]
    [string]$DiffOutput
)

$ErrorActionPreference = 'Stop'
$repositoryRoot = [System.IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..'))
$reviewerRoot = [System.IO.Path]::GetFullPath($ReviewerRoot)
$baseReportPath = [System.IO.Path]::GetFullPath($BaseReport, (Get-Location).Path)
$headOutputPath = [System.IO.Path]::GetFullPath($HeadOutput, (Get-Location).Path)
$headReviewOutputPath = [System.IO.Path]::GetFullPath($HeadReviewOutput, (Get-Location).Path)
$diffOutputPath = [System.IO.Path]::GetFullPath($DiffOutput, (Get-Location).Path)

if (-not (Test-Path -LiteralPath $baseReportPath -PathType Leaf)) {
    throw "Base snapshot report does not exist: $baseReportPath"
}

if (Test-Path -LiteralPath $diffOutputPath) {
    throw "Refusing to overwrite an existing diff report: $diffOutputPath"
}

& (Join-Path $PSScriptRoot 'Scan-SnapshotBase.ps1') `
    -ReviewerRoot $reviewerRoot `
    -BaseVersion V4 `
    -Output $headOutputPath `
    -ReviewOutput $headReviewOutputPath

$cliPath = Join-Path $reviewerRoot 'src\FeatBit.FeatureFlagReviewer.Cli\bin\Debug\net10.0\feature-flag-reviewer.dll'
if (-not (Test-Path -LiteralPath $cliPath -PathType Leaf)) {
    throw "Compiled SnapshotScanner CLI was not found: $cliPath"
}

$diffOutputDirectory = [System.IO.Path]::GetDirectoryName($diffOutputPath)
if (-not (Test-Path -LiteralPath $diffOutputDirectory -PathType Container)) {
    New-Item -ItemType Directory -Path $diffOutputDirectory | Out-Null
}

dotnet $cliPath diff `
    --base $baseReportPath `
    --head $headOutputPath `
    --output $diffOutputPath
if ($LASTEXITCODE -ne 0) {
    throw "Snapshot diff failed with exit code $LASTEXITCODE."
}

$baseHash = (Get-FileHash -LiteralPath $baseReportPath -Algorithm SHA256).Hash
$headHash = (Get-FileHash -LiteralPath $headOutputPath -Algorithm SHA256).Hash
$headReviewHash = (Get-FileHash -LiteralPath $headReviewOutputPath -Algorithm SHA256).Hash
$diffHash = (Get-FileHash -LiteralPath $diffOutputPath -Algorithm SHA256).Hash
Write-Host "Base snapshot: $baseReportPath"
Write-Host "Base SHA-256: $baseHash"
Write-Host "Head snapshot: $headOutputPath"
Write-Host "Head SHA-256: $headHash"
Write-Host "Head review: $headReviewOutputPath"
Write-Host "Head review SHA-256: $headReviewHash"
Write-Host "Diff report: $diffOutputPath"
Write-Host "Diff SHA-256: $diffHash"
Write-Host 'Read the Head Markdown and Diff JSON before making a review decision.'

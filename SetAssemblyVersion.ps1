param(
    [Parameter(Mandatory=$true, Position=0)]
    [string] $NewVersion
)

$scriptDir = Split-Path $script:MyInvocation.MyCommand.Path

Get-ChildItem -Path $scriptDir -Depth 2 -Filter "AssemblyInfo.cs" | %{
    (Get-Content -Path $_.FullName -Raw -Encoding UTF8) -creplace "(?<=\[assembly:\s*Assembly(?:File)?Version\("")[\d\.]+(?=""\)\])", $NewVersion |
        Set-Content -Path $_.FullName -NoNewline -Encoding UTF8
}
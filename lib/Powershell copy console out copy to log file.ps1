
$process = "node -e 'console.log(123)'"
$processForFileName = $process -replace "[\W]", "-"
$processForFileName = $processForFileName -replace "-+", "-"
$RunDate = (Get-Date).ToString("yyyyMMddHHmmss")
$RootPath = Resolve-Path -Path '.\'
$LogFile = "$($processForFileName)-$($RunDate).log"
$LogPath = Join-Path -Path $RootPath -ChildPath $LogFile

Write-Host "Starting $($process)"
Write-Host "Copying all console output to $($LogPath)"

# node .\index.js > "$($LogPath)"
# 2>&1 redirects messages and errors into standard out ( '>' only ignores errors)
# Tee-Object copies out to file while maintaining console out
Invoke-Expression $process 2>&1 | Tee-Object -file "$($LogPath)"
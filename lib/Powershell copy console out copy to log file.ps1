Function Get-Slug-Encoding {
    param ($InString)

    $InvalidPathChars = [IO.Path]::GetInvalidFileNameChars() -join ""
    $SlugOutChars = $InvalidPathChars + ":. '()="
    $RegExp = "[{0}]" -f [RegEx]::Escape($SlugOutChars)
    $OutString = $InString -replace $RegExp, "-"
    $OutString = $OutString -replace "-+", "-"
	$OutString = $OutString -replace "^-", ""
	$OutString = $OutString -replace "-$", ""
    return $OutString
}

$process = "node -e 'console.log(123)'"
$processForFileName = Get-Slug-Encoding $process
$RunDate = (Get-Date).ToString("yyyyMMddHHmmss")
$RootPath = Resolve-Path -Path ".\"
$LogFile = "$($processForFileName)-$($RunDate).log"
$LogPath = Join-Path -Path $RootPath -ChildPath $LogFile

$SuperLogFile = "$($processForFileName).log"
$SuperLogPath = Join-Path -Path $RootPath -ChildPath $SuperLogFile

Write-Host "Starting $($process)"
Write-Host "Copying all console output to $($LogPath)"
Write-Host "Also appending same console output to $($SuperLogPath)"

# Example of starting node program
# Sending out to 1 log file and console. Includes errors
Invoke-Expression $process 2>&1 | Tee-Object -file "$($LogPath)"

# Example of starting node program
# Sending output to 2 log files and console. Includes errors
# Appends to $SuperLogFile. Overwites $LogFile
Invoke-Expression $process 2>&1 | Tee-Object -Append -FilePath $SuperLogPath | Tee-Object -FilePath $LogPath
# Direct version
# node -e 'console.log(123)' 2>&1 | Tee-Object -Append -FilePath $SuperLogPath | Tee-Object -FilePath $LogPath
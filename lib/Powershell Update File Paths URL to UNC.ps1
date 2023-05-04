param(
	[string]$file,
	[string]$postConvertSearch,
	[string]$postConvertRepl
)

# Run with
# & ".\Powershell Update File Paths URL to UNC.ps1" "C:\Temp\MyFile.xml"

# Fix parameter globbing
if ($postConvertSearch -eq '') {
	"Fixing parameters"
	$pSplit = $file.split(' ')
	$file = $pSplit[0]
	$postConvertSearch = $pSplit[1]
	$postConvertRepl = $pSplit[2]
}

$preConvertPathRootSearch = "file:///"
$preConvertPathRootRepl = "//"

"Updating paths - URL style to UNC style"

"Reading '$file'"
$newContent = Get-Content -Path $file
$newFile = $file.Replace('.xml', '.over-network.xml')

"Replacing '$preConvertPathRootSearch' with '$preConvertPathRootRepl'"
$newContent = $newContent.Replace($preConvertPathRootSearch, $preConvertPathRootRepl)
"Replacing LF with CRLF"
$newContent = $newContent -replace '((?<!\r)\n|\r(?!\n))', '\r\n'
"Replacing xmltag '<\' with temp '<-'"
$newContent = $newContent.Replace('<\', '<-')
"Replacing all other forwardslashes with backslashes"
$newContent = $newContent.Replace('/', '\')
"Replacing xmltag temp '<-' with '<\'"
$newContent = $newContent.Replace('<-', '<\')
"Decoding URL encoded characters"
$newContent = [uri]::UnescapeDataString($newContent)

"Post convert, replacing '$postConvertSearch' with '$postConvertRepl'"
$newContent = $newContent.Replace($postConvertSearch, $postConvertRepl)

"Writing '$newFile'"
$newContent | Set-Content -Path $newFile

Write-Host "Done"

function Get-EnvSetting {

    param (
        $FilePath,
		$SettingName
    )

	$SearchPattern = "^$SettingName="
	$Settings = Select-String -LiteralPath $FilePath -Pattern $SearchPattern
	$SettingLine = $Settings.Line
	if ($SettingLine.Length -lt 1) { Return '' }
	$SettingLineParts = $SettingLine.Split('=')
	$QuotedValue = $SettingLineParts[$SettingLineParts.Count-1]
	$Value = $QuotedValue.Replace('"','')
	Return $Value

}

$env = "..\.env"

"**********************"
"Parsing file '$env'"
""

"Parsed .env key: ENVIRONMENT"
Get-EnvSetting $env 'ENVIRONMENT'
""

"Parsed .env key: HOST"
Get-EnvSetting $env 'HOST'
""

"Parsed .env key: NONEXISTING"
Get-EnvSetting $env 'NONEXISTING'
""

"Parsed .env key: LOG"
Get-EnvSetting $env 'LOG'
""

"Parsed .env key: SUPERLOG"
Get-EnvSetting $env 'SUPERLOG'
""

"Done"
"**********************"

# # Sample .ENV lines
# # Copy below .env file. Remove the first two chars "# " on every line
# ENVIRONMENT="Debug"
# HOST="MyComputer"
# LOG=".\resources-temp\App-Log-yyyyMMddHHmmss.log"
# # Super Log has same messages as Log, but appends each new run without rolling over between runs or dates
# SUPERLOG=".\resources-temp\App-Log.log"

function Get-EnvSetting {

    param (
        $FilePath,
		$SettingName
    )

	$Settings = Select-String -Path $FilePath -Pattern $SettingName
	if ($Settings.Matches.Count -lt 1) {
		Return ''
	}
	$SettingLine = $Settings.Line
	$SettingLineParts = $SettingLine.Split('=')
	$QuotedValue = $SettingLineParts[$SettingLineParts.Count-1]
	$Value = $QuotedValue.Replace('"','')

	Return $Value

}

Get-EnvSetting '..\.env' 'LOG'
Get-EnvSetting '..\.env' 'remoteSourcePort'
Get-EnvSetting '..\.env' 'nonExisiting'

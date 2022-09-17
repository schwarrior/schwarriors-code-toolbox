Function Get-Slug-Encoding {
    param ($InString)

    $InvalidPathChars = [IO.Path]::GetInvalidFileNameChars() -join ""
    $SlugOutChars = $InvalidPathChars + ":. "
    $RegExp = "[{0}]" -f [RegEx]::Escape($SlugOutChars)
    $OutString = $InString -replace $RegExp
    $OutString = $OutString -replace "-+", "-"
    return $OutString
}
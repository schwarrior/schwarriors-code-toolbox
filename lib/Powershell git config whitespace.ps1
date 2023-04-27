# Ref https://git-scm.com/book/pt-br/v2/Customizing-Git-Git-Configuration

# a dash prefix indicates OFF 
$Off = "-"
$On = ""

# 3 settings ON by default at install time
# blank-at-eol, which looks for spaces at the end of a line;
$BlankAtEol = $On + "blank-at-eol"
# blank-at-eof, which notices blank lines at the end of a file; and 
$BlankAtEof = $Off + "blank-at-eof"
# space-before-tab, which looks for spaces before tabs at the beginning of a line.
$SpaceBeforeTab = $On + "space-before-tab"

# 3 settings OFF by default at install time
# indent-with-non-tab, which looks for lines that begin with spaces instead of tabs and is controlled by the tabwidth option; 
$IndentWithNonTab = $On + "indent-with-non-tab"
# tab-in-indent, which watches for tabs in the indentation portion of a line; and 
$TabInIndent = $Off + "tab-in-indent"
# cr-at-eol, which tells Git that carriage returns at the end of lines are OK.
$CrAtEol = $On + "cr-at-eol"

"Configuring Git Step 1"
$GitConfig1 = "git config --global core.whitespace $($BlankAtEol),$($BlankAtEof),$($SpaceBeforeTab),$($IndentWithNonTab),$($TabInIndent),$($CrAtEol)"
Write-Host $GitConfig1
Invoke-Expression $GitConfig1

# Convert CRLF to LF on commit, Convert LF to CRLF on checkout
# $UnixEmuLF = "true"
# Convert CRLF to LF on commit, No changes on checkout
# $UnixPureLF = "input"
# No changes on commit, No changes on checkout for pure Windows environs
$WindowsCRLF = "false"
$AutoCrLf = $WindowsCRLF

"Configuring Git Step 2"
$GitConfig2 = "git config --global core.autocrlf $($AutoCrLf)"
Write-Host $GitConfig2
Invoke-Expression $GitConfig2

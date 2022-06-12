Combine text files from command line
====================================

With PowerShell, explicitly defining input files:

```powershell
Get-Content inputfile1.txt, inputfile2.txt | Set-Content joinedFile.txt
```

With PowerShell, using wildcards for input file names:

```powershell
Get-Content inputfile*.txt, inputfile*.txt | Set-Content joinedFile.txt
```

With Windows Command Prompt
```shell
copy 1.txt+2.txt 3.txt
```

Configuring Powershell Script Execution on a New Machine
==========================================================

Chck the local PowerShell Execution policy. Is it set to Unrestricted?

* Open a Powershell window on the target server:
```
Get-ExecutionPolicy
```

To allow unrestriced script execution:

* Open (another) PowerShell window as an administrator and type the following:
```
Set-ExecutionPolicy Unrestricted
```
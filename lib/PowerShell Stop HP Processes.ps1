# Run as an administrator

# HP spyware
Get-Process AppHelperCap -ErrorAction SilentlyContinue | Stop-Process -PassThru -Force # Restarts
Get-Process BridgeCommunication -ErrorAction SilentlyContinue | Stop-Process -PassThru -Force # Restarts
Get-Process DiagsCap -ErrorAction SilentlyContinue | Stop-Process -PassThru -Force # Restarts
Get-Process NetworkCap -ErrorAction SilentlyContinue | Stop-Process -PassThru -Force # Restarts
Get-Process SysInfoCap -ErrorAction SilentlyContinue | Stop-Process -PassThru -Force # Restarts
Get-Process TouchpointAnalyticsClientService -ErrorAction SilentlyContinue | Stop-Process -PassThru -Force # Restarts
# HP Omen gaming spyware
Get-Process LightStudio-background -ErrorAction SilentlyContinue | Stop-Process -PassThru -Force
Get-Process OmenCap -ErrorAction SilentlyContinue | Stop-Process -PassThru -Force
Get-Process OmenCommandCenterBackground -ErrorAction SilentlyContinue | Stop-Process -PassThru -Force
Get-Process OmenInstallMonitor -ErrorAction SilentlyContinue | Stop-Process -PassThru -Force
Get-Process omenmqtt -ErrorAction SilentlyContinue | Stop-Process -PassThru -Force
Get-Process OMENOverlay -ErrorAction SilentlyContinue | Stop-Process -PassThru -Force
Get-Process OverlayHelper -ErrorAction SilentlyContinue | Stop-Process -PassThru -Force

# Many of these processes will just restart after a few seconds. 
# How to keep many of these proceses from restarting
# https://www.wikihow.com/Block-an-Application-or-.EXE-from-Running-in-Windows
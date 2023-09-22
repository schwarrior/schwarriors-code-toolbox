Tip: Boost Angular Compile Speed
================================

Angular development on Windows is sluggish. 

The speed of Angular compilation via `ng serve` or `ng build` commands can be increased dramatically by disabling Windows Real-Time virus protection. 

To disable real-time scanning on Windows 11:

1. Open Windows Security app
2. Go to Virus & thread protection tab
3. Under Virus and threat protection settings heading, click the Manage Settings link.
4. Under Real-time protection heading, toggle switch to Off.

Alternative, permanent fix:

1. At the bottom of the Virus and threat protection settings page, under the Exclusions heading, click the Add or remove excusions link.
2. Add an exclusion for the folder location of the `NYGearUp2.NgApp` folder or a parent folder.
3. Add an exclusion for the process NPM.

requires cordova and ionic
```Shell
$ sudo npm install -g cordova
$ sudo npm install -g ionic
```

requires project to be stated. if there is already a ionic.project file then this has been done.
note this command will download template files
```Shell
$ ionic start SuperFlash blank
```

navigate to root of the project

to test for ios (must be on MacOS)
```Shell
$ ionic platform add ios
$ ionic build ios
$ ionic emulate ios
```

to test in browser (platform generic)
```Shell
$ ionic serve
```

upload to ionic view (to test on iphone) (login will be required)
```Shell
$ ionic upload
```

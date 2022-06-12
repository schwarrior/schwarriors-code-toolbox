requires firebase tools
```Shell
$ npm update -g firebase-tools
```

login
```Shell
$ firebase login
```

(Browser may launch, requiring authorization of client. click authorize)

requires project to be initialized. if there is already a firebase.json file then this has been done.
```Shell
$ firebase init
```
deploy (a login will be required)
```Shell
$ firebase deploy
```
check deployed site
https://sampleapp.firebaseapp.com
or
```Shell
$ firebase open
```
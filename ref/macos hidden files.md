MacOS Hidden Files and Folders
==============================


# Showing Hidden Files in Finder with Keystroke 

See hidden files on Mac via Finder 

1. In Finder, open up your Macintosh HD folder. 
2. Press Command+Shift+Dot. 
3. Your hidden files will become visible. Repeat step 2 to hide them again! 


# Go to a folder by entering its pathname 

Use this method to temporarily show content of Hidden folder 

- In the Finder, choose Go > Go to Folder. 
- Type the folder’s pathname (for example, /Library/Fonts/ or ~/Pictures/), then click Go. 
- A slash (/) at the beginning of a pathname indicates that the starting point is the top level of your computer’s folder structure. 
- A slash at the end indicates that this is a path to a folder, rather than a file. 
- A tilde (~) indicates your home folder. Most of your personal folders, such as Documents, Music, and Pictures, are in your home folder. 
- If you’re having trouble finding a folder, make sure you’re spelling its name correctly and typing the full path, including slashes. 


# Show Hidden Files with Terminal 

```
ls -a 
```
 
# Show Hidden Files with Batch File 

Add the following to ~/.zshrc

```
alias showFiles='defaults write com.apple.finder AppleShowAllFiles YES; killall Finder /System/Library/CoreServices/Finder.app'
alias hideFiles='defaults write com.apple.finder AppleShowAllFiles NO; killall Finder /System/Library/CoreServices/Finder.app'
```

Save the file anc exit text editor. New aliases will take effect after running terminal command:

```
source .zshrc
```

Then invoke from terminal 

```
showFiles 
hideFiles 
```


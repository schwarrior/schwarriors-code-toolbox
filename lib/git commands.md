Transform the current directory into a Git repository (only for local only non-share repos)
```Shell
$ git init
```

Create an empty Git repository in the specified directory (only for local only non-shared repos)
```Shell
$ git init <directory>
```

Create an empty Git repository in the specified directory (for local onlyshared repos. Bare means no working dir. Must be cloned to be worked with)
```Shell
$ git init --bare <directory>
```

Add a submodule into a subfolder
```Shell
$ git submodule add https://github.com/chaconinc/DbConnector
```

Update local repo and submodules
```Shell
$ git pull
$ git submodule update
```

List all branches (local and remote)
```Shell
$ git branch -a
```

Create Branch & Sync with remote
```Shell
$ git branch <feature_branch>
$ git push origin <feature_branch>
$ git branch --set-upstream-to=origin/<feature_branch> <feature_branch>
$ git checkout <feature_branch>

$ rem additional commands necessary to be able to run git pull and pull <feature_branch> by default
```

Merge branch with master & sync with remote
```Shell
$ git checkout master
$ git branch --set-upstream-to=origin/master master
$ git pull origin master
$ git merge <feature_branch>
$ git push origin master

```

Push a local Repo to a new remote origin
```Shell
git remote add origin https://gitbub.com/sampleuser/newremoteorigin
git push -u origin --all
```
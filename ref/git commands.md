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

Query the current directories remote URL
```Shell
git remote -v
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

Merge feature-branch into main
```Shell
$ git checkout feature-branch
$ # if error, branch must be created locally with $ git branch feature-branch # then rerun checkout
$ git push
$ git pull
$ # if push or pull issues run $ git branch --set-upstream-to=origin/feature-branch feature-branch # then rerun push and pull
$ git checkout main
$ # if error, branch must be created locally with $ git branch main # then rerun checkout
$ git push
$ git pull
$ # if push or pull issues run $ git branch --set-upstream-to=origin/main main # then rerun push and pull
$ git merge feature-branch
$ # check source for any merge conflicts and correct
$ # if corrections $ git commit -am "resolve conflicts from merge of feature-branch into main"
$ git push
```

Push a local Repo to a new remote origin
```Shell
git remote add origin https://gitbub.com/sampleuser/newremoteorigin
git push -u origin --all
```

Set name and email
```Shell
git config --global user.name "User Name"
git config --global user.name "user@emaildomain.com"
```

Make CRLF lines and other Windows whitespace standards OK.
```Shell
git config --global core.whitespace blank-at-eol,-blank-at-eof,space-before-tab,indent-with-non-tab,-tab-in-indent,cr-at-eol
```

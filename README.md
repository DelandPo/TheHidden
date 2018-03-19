# Git Setup

 - Install Git from https://git-scm.com/downloads
 - Learn more about Git https://git-scm.com/book/en/v1/Getting-Started-About-Version-Control

# How to get started

 1. Open a Bash/Powershell and create a folder using mkdir TheHidden
 2. cd TheHidden
 3. git init
 4. git remote add origin https://github.com/DelandPo/TheHidden.git
 5. git remote -v
 6. git pull origin development
 7. Now you should have unity project on local machine
 8. git push --set-upstream origin development
 9. git push
 10. From Unity open the project
 

## What to do after making changes

 1. git add --all
 2. git commit -m "Helpful messages"
 3. git push
 

## Create and switch to another branch
		

 1. Create a branch -> git branch [branch-name]
 2. Switch to specified branches and updates the directory -> git checkout [branch-name] 
 3. Combine specified branch with current one -> git merge [branch-name]
 4. Delete a branch -> git branch -d [branch-name]

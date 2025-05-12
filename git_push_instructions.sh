#!/bin/bash

# Initialize git repository if not already initialized
if [ ! -d .git ]; then
  git init
  echo "Git repository initialized."
else
  echo "Git repository already exists."
fi

# Add all files to git
git add .

# Create initial commit
git commit -m "Initial commit of DotNetNuke codebase"

# Add the remote repository
git remote add origin https://github.com/BoweyLou/DNN0409.git

# Push to the repository
git push -u origin master

echo "Done! Check the output above for any errors."
echo "If you encounter authentication issues, make sure you have proper credentials set up."
echo "You might need to use a personal access token if you're using HTTPS."
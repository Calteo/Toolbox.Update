# @variables

# Main 
Wait-Process -Id $process -ErrorAction SilentlyContinue

# @installers

# execute
Start-Process $executable $arguments -ErrorAction SilentlyContinue

# remove all
Remove-Item $PSScriptRoot -Recurse -Force	

Remove-Item "$target\*" -Recurse -Force
Copy-Item -Path "$source\*" -Destination $target -Recurse


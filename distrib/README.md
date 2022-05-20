### Create new release

```
# 1. Update project version
./update_version.sh <version>

# 2. Build project with vs studio...

# 3. Create packaging directory
./create_package_dir.sh <version>

# 4. Create yak package (exec on windows!)
./build_yak.bat

# 5. Upload yak to repo (exec on windows!)
./publish_yak.bat

# 6. Package can be installed in rhino with _Package_Manger, then search for Plugin
```


- https://developer.rhino3d.com/guides/yak/pushing-a-package-to-the-server/
- https://developer.rhino3d.com/guides/yak/the-anatomy-of-a-package/
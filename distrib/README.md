### Create new release

```
# 1. Update project version
./update_version.sh <version>

# 2. Build project with vs studio...

# 3. Create packaging directory (XXX: We copy from DEBUG not RELEASE currently)
./create_package_dir.sh <version>

# 4. Create yak package (exec on windows!)
./build_yak.bat

# 5. Upload yak to repo (exec on windows!)
./publish_yak.bat

# 6. Package can be installed in rhino with PackageManager, then search for Plugin
```


- https://developer.rhino3d.com/guides/yak/pushing-a-package-to-the-server/
- https://developer.rhino3d.com/guides/yak/the-anatomy-of-a-package/
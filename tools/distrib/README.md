## Tools

This document describes the setup tools to build, package and deploy strucenglib
on linux. All dependencies are captured in the provided vagrant box.

```
# boot vagrant and login
cd /project/root/dir
vagrant up
vagrant ssh

# cd into distrib tools
cd /vagrant/tools/distrib

# build the dotnet solution
./distrib.sh build

# Package into yak format
./distrib.sh package
```
The built Rhino plugin can susequently be found in `./StrucEngLib/bin/Debug/net48/`.
The yak package can be found in `./distrib/build`

### Publish to Rhino Store
Log into yak with wine yak.exe login. Your mcneel account must be connected with
strucenglib.

```sh
vagrant ssh
cd /vagrant/tools/distrib
wine yak.exe login
```

Publish to test store
```sh
vagrant ssh
cd /vagrant/tools/distrib
./distrib.sh distrib_test
```

Publish to store
```sh
vagrant ssh
cd /vagrant/tools/distrib
./distrib.sh distrib
```


### Links
- In case wine has issues publishing the yak file, update dotnet binary as described here
https://web.archive.org/save/https://gist.github.com/abertschi/464c5143f0290572711bb909b610208d

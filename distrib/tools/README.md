## Tools

This document describes the setup tools to build, package and deploy strucenglib
on linux.

- Install Wine https://www.winehq.org/
- Install dotnet https://docs.microsoft.com/en-us/dotnet/core/install/linux-ubuntu
- install xmlstarlet


Package management is done with Yak.exe by mcneel, freely distributed at
https://files.mcneel.com/yak/tools/latest/yak.exe.


### Getting Started
Log into yak with wine yak.exe login. Your mcneel account must be connected with
strucenglib.

```sh
wine yak.exe login
```

```sh
./distrib.sh help
```


### Issues
- In case wine has issues publishing the yak file, update dotnet binary as described here
https://web.archive.org/save/https://gist.github.com/abertschi/464c5143f0290572711bb909b610208d

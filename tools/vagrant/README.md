## Reproducible Build with Vagrant

### Linux

The development build of this project is captured in a vagrant linux box. For a
reproducible setup on linux, install the following binaries:

- vagrant https://www.vagrantup.com/
-  virtualbox https://www.virtualbox.org/.
 
Upon installation, run `vagrant up` in the root directory of this project. A virtual machine running fedora and all
build packages will set up. Release management can subsequently be done with
`/tools/distrib` in the project root directory. This setup will cross compile strucenglib on linux for windows.

The official build system for this project is Linux.
Wine is needed to run yak.exe on Linux, Rhino's Package Manager.

``` sh
# boot box
vagrant up

# build solution
/tools/distrib/distrib_vagrant.sh build
```

### Windows
On windows, install Rhino, dotnet and visual studio, import the project in
visual studio and build the solution with "build".

Consider Rhino's guide how to bootstrap a hello world project.
https://developer.rhino3d.com/guides/rhinocommon/your-first-plugin-windows/

## Reproducible Build

### Linux (Recommended)

The development build of this project is captured in a vagrant linux box. For a
reproducible setup on linux, install the following binaries:

- vagrant https://www.vagrantup.com/
- virtualbox https://www.virtualbox.org/.
 
Upon installation, run the following commands to build the project.

``` sh
# boot box
vagrant up

# build solution
/tools/distrib/distrib_vagrant.sh help
/tools/distrib/distrib_vagrant.sh build
```

This setup will cross compile strucenglib on linux for windows.  
The official build system for this project is Linux.

### Windows
If you prefer to build on windows, install Rhino, dotnet and visual studio, import the project in
visual studio and build the solution with "build".

Consider Rhino's guide how to bootstrap a hello world project.
https://developer.rhino3d.com/guides/rhinocommon/your-first-plugin-windows/

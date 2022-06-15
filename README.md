[![CI](https://github.com/kfmResearch-NumericsTeam/StrucEng_Library_Plug_in/actions/workflows/build_solution.yml/badge.svg?branch=master)](https://github.com/kfmResearch-NumericsTeam/StrucEng_Library_Plug_in/actions/workflows/build_solution.yml) 

# StrucEngLib Plugin

### Installation
To install, search for "StrucEngLib" in Rhino's Package Store. Please consider
the [installation section in the
wiki](https://github.com/kfmResearch-NumericsTeam/StrucEng_Library_Plug_in/wiki/Installation)
for more detailed instructions.

### Build
The build setup is specified and reproducible with a Fedora based Vagrant image.
Please consider [./tools/vagrant](./tools/vagrant) for detailed build instructions. 
```
$ vagrant up
$ ./tools/distrib/distrib_vagrant.sh help
$ ./tools/distrib/distrib_vagrant.sh build
$ ./tools/distrib/distrib_vagrant.sh test
$ ./tools/distrib/distrib_vagrant.sh package
$ ./tools/distrib/distrib_vagrant.sh deploy
```
### Files
```
./StrucEngLib/...........: .net C# Rhino Plugin
./StrucEngLibTest/.......: Tests
./distrib/...............: Assets files for Distribution management, logo, manifest...
./tools/.................: Development tools
./tools/vagrant/.........: Vagrant documentation and scripts
./tools/distrib/.........: Build and distribution tools
./tools/test/............: Unit test runner
```

[![Continuous Integration](https://github.com/kfmResearch-NumericsTeam/StrucEng_Library_Plug_in/actions/workflows/distrib_sh_build.yml/badge.svg?branch=master)](https://github.com/kfmResearch-NumericsTeam/StrucEng_Library_Plug_in/actions/workflows/distrib_sh_build.yml)
# StrucEngLib Plugin
> Rhinoceros 3D Plugin For StrucEng Library.   
> The StrucEng Library includes mechanical models, saftey concepts, GUI's, load generator, etc. for the strucutral analysis reinforced concrete and masonry.

  
<p align="left">
    <img src="./.github/strucenglib_gh.png" alt="strucenglib" width="700"/>
</p>

### Installation
To install, search for "StrucEngLib" in Rhino's Package Store. Please consider
the [installation section in the
wiki](https://github.com/kfmResearch-NumericsTeam/StrucEng_Library_Plug_in/wiki/Installation)
for more detailed instructions.

### Build
The build setup is specified and reproducible with a linux based Vagrant image.
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
./tools/.................: Development tools
./tools/vagrant/.........: Vagrant documentation and scripts
./tools/distrib/.........: Build and distribution tools
./tools/distrib/assets...: Asset files for distribution, manifest
./tools/test/............: Unit test runner
```

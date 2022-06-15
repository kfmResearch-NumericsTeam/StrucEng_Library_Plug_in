#!/usr/bin/env bash

# setup script for fedora vagrant box

# .net
sudo dnf install -y dotnet-sdk-6.0
sudo dnf install -y aspnetcore-runtime-6.0
sudo dnf install -y dotnet-runtime-6.0
sudo dnf install -y mono-devel

#wine
sudo dnf -y install dnf-plugins-core
sudo dnf -y install wine wine-devel
sudo dnf -y install winetricks

winetricks -q dotnet48

# other tools for distrib.sh
sudo dnf -y install xmlstarlet

# check build
/vagrant/tools/distrib/distrib.sh version
/vagrant/tools/distrib/distrib.sh build
/vagrant/tools/distrib/distrib.sh package

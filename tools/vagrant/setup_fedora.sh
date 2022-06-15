#!/usr/bin/env bash
set -euo pipefail

# setup script for fedora vagrant box

sudo dnf -y update

# .net
sudo dnf install -y dotnet-sdk-6.0
# sudo dnf install -y dotnet-runtime-6.0
sudo dnf install -y mono-devel

#wine
sudo dnf -y install dnf-plugins-core
sudo dnf -y install wine

# Not needed
# sudo dnf -y install winetricks
# winetricks -q dotnet48

# other tools for distrib.sh
sudo dnf -y install xmlstarlet

# check build
/vagrant/tools/distrib/distrib.sh version
/vagrant/tools/distrib/distrib.sh build
/vagrant/tools/distrib/distrib.sh package

echo "Setup successful!"

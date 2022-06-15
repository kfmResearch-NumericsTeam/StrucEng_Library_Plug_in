#!/usr/bin/env bash
set -euo pipefail

# setup script for fedora vagrant box
sudo dnf -y update --nogpgcheck || true
sudo dnf -y upgrade --nogpgcheck || true
sudo dnf install -y --nogpgcheck dnf-plugins-core

# .net
sudo dnf install -y --nogpgcheck dotnet-sdk-6.0
sudo dnf install -y --nogpgcheck dotnet-runtime-6.0
sudo dnf install -y --nogpgcheck mono-devel

# other tools for distrib.sh
sudo dnf -y install --nogpgcheck xmlstarlet

# check build
/vagrant/tools/distrib/distrib.sh version
/vagrant/tools/distrib/distrib.sh build
/vagrant/tools/distrib/distrib.sh package
/vagrant/tools/distrib/distrib.sh test

echo "Setup successful!"

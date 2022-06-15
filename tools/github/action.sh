#!/usr/bin/env bash
set -euo pipefail

#
# ubuntu 20.04 lts
#

sudo mkdir -p /etc/gcrypt
sudo su root -c "echo all >> /etc/gcrypt/hwf.deny"
sudo apt-get upgrade -y || true
sudo apt-get update -y || true

# .net https://docs.microsoft.com/en-us/dotnet/core/install/linux-ubuntu#2204
wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb
sudo apt-get update || true
sudo apt-get install -y apt-transport-https
sudo apt-get update || true
sudo apt-get install -y dotnet-sdk-6.0
sudo apt-get update || true
sudo apt-get install -y dotnet-runtime-6.0

# mono https://www.mono-project.com/download/stable/
sudo apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF
echo "deb https://download.mono-project.com/repo/ubuntu stable-focal main" | sudo tee /etc/apt/sources.list.d/mono-official-stable.list
sudo apt update
sudo apt install -y mono-devel

# wine https://wine.htmlvalidator.com/install-wine-on-ubuntu-20.04.html
#sudo rm -rf /var/lib/apt/lists/*
#sudo apt-get update -o Acquire::CompressionTypes::Order::=gz
#sudo apt-get update && sudo apt-get upgrade
#
#sudo dpkg --add-architecture i386 
#wget -nc https://dl.winehq.org/wine-builds/winehq.key
#sudo mv winehq.key /usr/share/keyrings/winehq-archive.key
#wget -nc https://dl.winehq.org/wine-builds/ubuntu/dists/focal/winehq-focal.sources
#sudo mv winehq-focal.sources /etc/apt/sources.list.d/
#sudo apt update -y
#sudo apt --fix-broken install -y --install-recommends winehq-devel
#wine --version

## other tools for distrib.sh
sudo apt-get install -y xmlstarlet

## check build
/vagrant/tools/distrib/distrib.sh version
/vagrant/tools/distrib/distrib.sh build
/vagrant/tools/distrib/distrib.sh package
/vagrant/tools/distrib/distrib.sh test

echo "Setup successful!"

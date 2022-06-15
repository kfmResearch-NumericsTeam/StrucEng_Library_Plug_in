#!/usr/bin/env bash
#
# ubuntu 20.04 lts
#

set -euo pipefail
script_dir=$( cd -- "$( dirname -- "${BASH_SOURCE[0]}" )" &> /dev/null && pwd )
proj_root="$script_dir/../.."

sudo apt-get upgrade -y || true
sudo apt-get update -y || true

sudo apt-get install -y unzip

# .net https://docs.microsoft.com/en-us/dotnet/core/install/linux-ubuntu#2204
wget https://packages.microsoft.com/config/ubuntu/18.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb

sudo apt-get update; \
  sudo apt-get install -y apt-transport-https && \
  sudo apt-get update && \
  sudo apt-get install -y dotnet-sdk-6.0
  sudo apt-get update; \
  sudo apt-get install -y apt-transport-https && \
  sudo apt-get update && \
  sudo apt-get install -y dotnet-runtime-6.0
  
# mono https://www.mono-project.com/download/stable/
sudo apt install -y gnupg ca-certificates
sudo apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF
echo "deb https://download.mono-project.com/repo/ubuntu stable-bionic main" | sudo tee /etc/apt/sources.list.d/mono-official-stable.list
sudo apt update -y
sudo apt --fix-broken install -y  mono-devel

# wine
sudo dpkg --add-architecture i386 
wget -nc https://dl.winehq.org/wine-builds/winehq.key
sudo mv winehq.key /usr/share/keyrings/winehq-archive.key
wget -nc https://dl.winehq.org/wine-builds/ubuntu/dists/bionic/winehq-bionic.sources
sudo mv winehq-bionic.sources /etc/apt/sources.list.d/
sudo apt-get update
sudo apt install -y --install-recommends winehq-stable
sudo apt-get install -y winetricks
winetricks -q dotnet46
wine --version

## other tools for distrib.sh
sudo apt-get install -y xmlstarlet

## check build 
$proj_root/tools/distrib/distrib.sh version
$proj_root/tools/distrib/distrib.sh build
$proj_root/tools/distrib/distrib.sh package || true
$proj_root/tools/distrib/distrib.sh test

echo "Setup successful!"

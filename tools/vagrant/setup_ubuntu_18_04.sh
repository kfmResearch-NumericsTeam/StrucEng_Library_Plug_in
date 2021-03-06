#!/bin/bash
#
# for ubuntu 18.04
# also used in github action
#
set -euo pipefail
script_dir="$( cd -- "$( dirname -- "${BASH_SOURCE[0]:-$0}"; )" &> /dev/null && pwd 2> /dev/null; )";
proj_root="$script_dir/../.."

sudo mkdir -p /etc/gcrypt
sudo su root -c "echo all >> /etc/gcrypt/hwf.deny"

sudo apt-get upgrade -y || true
sudo apt-get update -y || true
sudo apt-get install -y unzip

# .net https://docs.microsoft.com/en-us/dotnet/core/install/linux-ubuntu
wget https://packages.microsoft.com/config/ubuntu/18.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb
sudo apt-get update || true
sudo apt-get install -y apt-transport-https dotnet-sdk-6.0 dotnet-runtime-6.0

# Install powershell
sudo apt-get install -y wget apt-transport-https software-properties-common
wget -q "https://packages.microsoft.com/config/ubuntu/$(lsb_release -rs)/packages-microsoft-prod.deb"
sudo dpkg -i packages-microsoft-prod.deb
sudo apt-get update -y
sudo apt-get install -y powershell


# mono https://www.mono-project.com/download/stable/
sudo apt install -y gnupg ca-certificates
sudo apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF
echo "deb https://download.mono-project.com/repo/ubuntu stable-bionic main" | sudo tee /etc/apt/sources.list.d/mono-official-stable.list
sudo apt update || true
sudo apt install -y mono-devel

## other tools for distrib.sh
sudo apt-get install -y xmlstarlet

# check build
$proj_root/tools/distrib/distrib.sh version
$proj_root/tools/distrib/distrib.sh build
$proj_root/tools/distrib/distrib.sh package
$proj_root/tools/distrib/distrib.sh test

echo "Setup successful!"

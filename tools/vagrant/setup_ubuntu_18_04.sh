#!/usr/bin/env bash
#
# for ubuntu 18.04
# also used in github action
#
set -euo pipefail
script_dir=$( cd -- "$( dirname -- "${BASH_SOURCE[0]}" )" &> /dev/null && pwd )
proj_root="$script_dir/../.."

sudo mkdir -p /etc/gcrypt
sudo su root -c "echo all >> /etc/gcrypt/hwf.deny"
sudo apt-get upgrade -y || true
sudo apt-get update -y || true

# .net https://docs.microsoft.com/en-us/dotnet/core/install/linux-ubuntu
wget https://packages.microsoft.com/config/ubuntu/18.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb
sudo apt-get update || true
sudo apt-get install -y apt-transport-https
sudo apt-get update || true
sudo apt-get install -y dotnet-sdk-6.0
sudo apt-get update || true
sudo apt-get install -y dotnet-runtime-6.0

# mono https://www.mono-project.com/download/stable/
sudo apt install -y gnupg ca-certificates
sudo apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF
echo "deb https://download.mono-project.com/repo/ubuntu stable-bionic main" | sudo tee /etc/apt/sources.list.d/mono-official-stable.list
sudo apt update
sudo apt install -y mono-devel

## other tools for distrib.sh
sudo apt-get install -y xmlstarlet

## check build
$proj_root/tools/distrib/distrib.sh version
$proj_root/tools/distrib/distrib.sh build
$proj_root/tools/distrib/distrib.sh package
$proj_root/tools/distrib/distrib.sh test

echo "Setup successful!"
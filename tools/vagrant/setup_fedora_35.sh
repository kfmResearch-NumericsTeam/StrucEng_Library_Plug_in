#!/usr/bin/env bash
set -euo pipefail
script_dir=$( cd -- "$( dirname -- "${BASH_SOURCE[0]}" )" &> /dev/null && pwd )
proj_root="$script_dir/../.."

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
$proj_root/tools/distrib/distrib.sh version
$proj_root/tools/distrib/distrib.sh build
$proj_root/tools/distrib/distrib.sh package
$proj_root/tools/distrib/distrib.sh test

echo "Setup successful!"

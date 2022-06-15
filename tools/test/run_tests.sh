#!/usr/bin/env bash

#
# dotnet test fails on linux
# which is why we manually run test with nunit runner
#

set -euo pipefail
script_dir=$( cd -- "$( dirname -- "${BASH_SOURCE[0]}" )" &> /dev/null && pwd )
runner_dir=$script_dir/nunit-console-runner
runner=$runner_dir/tools/nunit3-console.exe
proj_root=$script_dir/../..
test_ddl=$proj_root/StrucEngLibTest/bin/Debug/net48/StrucEngLibTest.dll

ensure_binary() {
    local cmd="$1"
    if ! command -v "$cmd" &> /dev/null
    then
        echo "$cmd could not be found"
        exit
    fi
}

ensure_binary dotnet
ensure_binary mono

# unzip
unzip -u -n $script_dir/nunit-console-runner.3.15.0.nupkg -d $runner_dir

# build solution
dotnet build $proj_root

# run nunit tests with nunit runner
mono $runner $test_ddl /framework mono-4.0


 
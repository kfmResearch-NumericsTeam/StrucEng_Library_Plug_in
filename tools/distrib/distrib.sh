#!/bin/bash
set -euo pipefail

#
# Build script to build dotnet project, and create release artifacts
# See ensure_binaries for binary dependencies
#

WINE_MONO_TRACE=x
WINE_MONO_TRACE=E:System.NotImplementedException
MONO_INLINELIMIT=0

script_dir=$( cd -- "$( dirname -- "${BASH_SOURCE[0]}" )" &> /dev/null && pwd )
proj_root="$script_dir/../../"
yak_bin="wine $script_dir/yak.exe"


ensure_binaries() {
    ensure_binary xmlstarlet
    ensure_binary dotnet
    ensure_binary wine
}


ensure_binary() {
    local cmd="$1"
    if ! command -v "$cmd" &> /dev/null
    then
        echo "$cmd could not be found"
        exit 1
    fi
}

version() {
    local f1="$proj_root/StrucEngLib/StrucEngLib.csproj"

    xmlstarlet sel -t -m "//Version[1]"  -v . -n $f1
}

update_version() {
    local version=${1:-}
    if [ -z "$version" ]
    then
        echo "No argument set. $0 <version>"
        exit 1
    fi

    local f1="$proj_root/StrucEngLib/src/StrucEngLibPlugin.cs"
    local f2="$proj_root/StrucEngLib/StrucEngLib.csproj"

    echo "updating version to $version ..."
    sed -i "s/Version =.*/Version = \"$version\";/" "$f1"
    cat "$f1" | grep Version
    xmlstarlet ed --inplace -u "//Version[1]" -v "$version" "$f2"
    cat "$f2" | grep Version
}

build() {
    # Builds .net project
    echo "build..."
    dotnet build "$proj_root"
}

create_package_dir() {
    echo "create_package_dir..."

    local version=${1:-}
    if [ -z "$version" ]
    then
        echo "No argument set. $0 <version>"
        exit 1
    fi

    local bin="$proj_root/StrucEngLib/bin/Debug/net48"
    local out="$proj_root/distrib/build"
    local assets="$proj_root/distrib/assets"

    # Copy relevant bits
    rm -rf "$out"
    mkdir -p "$out"
    cp -r "$assets/"* $out
    cp "$proj_root/LICENSE" "$out"
    cp "$proj_root/README.md" "$out"
    cp -rf "$bin/"* "$out" 2>/dev/null

    sed -i "s/__VERSION__/$version/g" "$out/manifest.yml"

    ls -al "$out"
    cat "$out/manifest.yml"
}

package() {
    echo "package..."

    build

    local v=$(version)
    create_package_dir "$v"

    local _cd=$(pwd)
    cd "$proj_root/distrib/build"

    $yak_bin build
    printf '\033[0m'  # Reset color

    cd "$_cd"
}

deploy() {
    echo "deploy..."

    local _cd=$(pwd)
    cd "$proj_root/distrib/build"

    local yak_out=$(ls | grep yak)

    set -x
    $yak_bin push $yak_out
    set +x

    cd "$_cd"
}

deploy_test() {
    echo "deploy to test repo..."

    local test_repo="--source=https://test.yak.rhino3d.com"
    local _cd=$(pwd)
    cd "$proj_root/distrib/build"

    local yak_out=$(ls | grep yak)

    set -x
    $yak_bin push $test_repo $yak_out
    set +x

    cd "$_cd"
}


distrib_test() {
    echo "distrib_test..."

    local version=${1:-}
    local interactive=${2:-yes}
    if [ -z "$version" ]
    then
        echo "No argument set. $0 <version>"
        exit 1
    fi

    update_version "$version"
    build
    create_package_dir "$version"
    package

    if [ "$interactive" == "yes" ]
    then
        read -p "Press enter to deploy"
    fi

    deploy_test
}

unit_test() {
    echo "Unit testing..."
    local test_bin=$proj_root/tools/test/run_tests.sh

    $test_bin
}

ci_build() {
    local datetag=$(date +"%Y-%m-%dT%H-%M-%S%z")
    local v="-develop-$datetag"
    local version=$(version | cut -d'-' -f1)
    local interactive="yes"

    version="${version}${v}"

    update_version "$version"
    build
    create_package_dir "$version"
    package

    if [ "$interactive" == "yes" ]
    then
        read -p "Press enter to deploy"
    fi

    deploy_test
}

help() {
    echo "distrib.sh: $0 {update_version|version|build|package|deploy_test|deploy|distrib|distrib_test}" >&2
    echo "commands: " >&2
    echo "  update_version <version>.....: updates version" >&2
    echo "  version......................: list version" >&2
    echo "  build........................: build dotnet solution" >&2
    echo "  test.........................: build dotnet solution, run tests" >&2
    echo "  package......................: builds solution, creates yak package format" >&2
    echo "  deploy_test..................: deploys the yak package found to test store" >&2
    echo "  deploy.......................: deploys the yak package found store" >&2
    echo "  distrib......................: builds, packages, deploys package to store" >&2
    echo "  distrib_test.................: builds, packages, deploys package to test store" >&2
}

ensure_binaries

command=${1:-}
case "$command" in
    version)
        version
        ;;
    update_version)
        version=${2:-}
        if [ -z "$version" ]
        then
            echo "No argument set. $0 $1 <version>"
            exit 1
        fi
        update_version "$version"
        ;;
    build)
        build
        ;;
    test)
        unit_test
        ;;
    deploy_test)
        deploy_test
        ;;
    deploy)
        deploy
        ;;
    package)
        package
        ;;
    ci_build)
        ci_build
        ;;
    distrib)
        version=${2:-}
        interactive=${3:-yes}
        if [ -z "$version" ]
        then
            echo "No argument set. $0 $1 <version>"
            exit 1
        fi
        distrib "$version" "$interactive"
        ;;
    distrib_test)
        version=${2:-}
        interactive=${3:-yes}
        if [ -z "$version" ]
        then
            echo "No argument set. $0 $1 <version>"
            exit 1
        fi
        distrib_test "$version" "$interactive"
        ;;
    create_package_dir)
        version=${2:-}
        if [ -z "$version" ]
        then
            echo "No argument set. $0 $1 <version>"
            exit 1
        fi
        create_package_dir "$version"
        ;;
    *)
        help
        exit 2
esac

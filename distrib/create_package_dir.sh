#!/bin/bash
#
# Create the ./build directory with all assets
#
DIR=$( cd -- "$( dirname -- "${BASH_SOURCE[0]}" )" &> /dev/null && pwd )
BIN=$DIR/../StrucEngLib/bin/Debug/net48
OUT=$DIR/build

if [ -z "$1" ] 
then
    echo "No argument set. $0 <version>"
    exit 1
fi

VERSION="$1"

rm -rf $OUT
mkdir -p $OUT
cp -r $DIR/assets/* $OUT
cp $DIR/../LICENSE $OUT
cp $DIR/../README.md $OUT
cp -rf $BIN/* $OUT 2>/dev/null

sed -i "s/__VERSION__/$VERSION/g" $OUT/manifest.yml

ls -al $OUT


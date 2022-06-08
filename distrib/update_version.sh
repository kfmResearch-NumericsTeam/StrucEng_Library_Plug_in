#!/bin/bash
#
# Update versions in files
#

DIR=$( cd -- "$( dirname -- "${BASH_SOURCE[0]}" )" &> /dev/null && pwd )
if [ -z "$1" ] 
then
    echo "No argument set. $0 <version>"
    exit 1
fi

VERSION="$1"

sed -i "s/Version =.*/Version = \"$VERSION\";/" "$DIR/../StrucEngLib/src/StrucEngLibPlugin.cs"
cat "$DIR/../StrucEngLib/src/StrucEngLibPlugin.cs" | grep Version
xmlstarlet ed --inplace -u "//Version[1]" -v $VERSION "$DIR/../StrucEngLib/StrucEngLib.csproj"
cat "$DIR/../StrucEngLib/StrucEngLib.csproj" | grep Version
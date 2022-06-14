@Rem
@Rem Publishes the package from ./build
@Rem

@echo off
set YAK="C:\Program Files\Rhino 7\System\Yak.exe"
set TEST_REPO="--source=https://test.yak.rhino3d.com"
@Rem set TEST_REPO=""

cd build
dir /b /a-d *.yak > tmpFile 
set /p target= < tmpFile 
del tmpFile
echo %target%

%YAK% push %TEST_REPO% %target%

cd ..

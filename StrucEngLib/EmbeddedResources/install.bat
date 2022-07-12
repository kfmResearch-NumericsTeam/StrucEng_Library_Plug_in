@echo off
echo This installs compas and dependencies for StrucEngLib. It may take a while.

@echo on
set conda="%1"
set cenv=strucenglib3

call %conda% create -n %cenv% -c conda-forge python=3.9 compas --yes
call %conda% activate %cenv%
call pip install compas_fea

call python -m compas_rhino.install -v 7.0
call python -m compas_rhino.install -v 7.0 -p compas_fea

Rem Install SandwichModel
pip install https://github.com/kfmResearch-NumericsTeam/StrucEng_Library/archive/strucenglib_plugin.zip#subdirectory=Sandwichmodel
call python -m compas_rhino.install -v 7.0 -p Sandwichmodel
call python -m compas_rhino.install -v 7.0 -p Printerfunctions

@echo off
echo 
echo If something broke, try to delete conda environment %cenv% and retry
echo Installation finished. Press any key to exit . . .
pause>nul
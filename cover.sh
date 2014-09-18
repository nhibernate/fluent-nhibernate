#!/bin/bash

echo '##teamcity[dotNetCoverage]'

cd coverage/settings

echo Running unit tests with coverage
echo Searching for dotCover executable...

TC_DOTCOVER_PATH=${1:-"/c/TeamCity/buildAgent/tools/dotCover/dotCover.exe"}
echo Using dotCover from ${TC_DOTCOVER_PATH}

echo Running coverage...
${TC_DOTCOVER_PATH} cover nunit-coverage.xml
${TC_DOTCOVER_PATH} cover mspec-coverage.xml

echo Done, reporting to TeamCity...

echo "##teamcity[importData type='dotNetCoverage' tool='dotcover' path='coverage/results/NUnitOutput.xml']"
echo "##teamcity[importData type='dotNetCoverage' tool='dotcover' path='coverage/results/MSpecOutput.xml']"
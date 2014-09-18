#!/bin/bash

echo '##teamcity[dotNetCoverage]'

cd coverage/settings
echo Now in $PWD

echo Running unit tests with coverage
echo Searching for dotCover executable...

TC_DOTCOVER_PATH=${1:-"/c/TeamCity/buildAgent/tools/dotCover/dotCover.exe"}
echo Using dotCover from ${TC_DOTCOVER_PATH}

echo Running coverage...
${TC_DOTCOVER_PATH} cover nunit-coverage.xml
${TC_DOTCOVER_PATH} cover mspec-coverage.xml

echo Producing xml report...
${TC_DOTCOVER_PATH} merge merge-coverage.xml
${TC_DOTCOVER_PATH} report reporting.xml

echo Done, reporting to TeamCity...

echo '##teamcity[importData type='dotNetCoverage' tool='dotcover' path='coverage/results/report.xml']'
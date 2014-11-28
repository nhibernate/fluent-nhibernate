#!/bin/bash

echo '##teamcity[dotNetCoverage]'

cd coverage/settings

echo Running unit tests with coverage
echo Searching for dotCover executable...

MSPEC_PATH="..\\..\\src\\packages\\Machine.Specifications.0.5.15\\tools\\mspec${2}.exe"
NUNIT_PATH="..\\..\\src\\packages\\NUnit.2.5.7.10213\\Tools\\nunit-console-x86.exe"

echo Using MSpec from ${MSPEC_PATH}
echo Using NUnit from ${NUNIT_PATH}

DOTCOVER_PATH=${1:-"/c/TeamCity/buildAgent/tools/dotCover/dotCover.exe"}
echo Using dotCover from ${DOTCOVER_PATH}

echo Running coverage...
${DOTCOVER_PATH} cover nunit-coverage.xml //TargetExecutable=${NUNIT_PATH} //TargetArguments="FluentNHibernate.Testing.dll /framework:${3} /xml:../results/NUnitResult.xml"
${DOTCOVER_PATH} cover mspec-coverage.xml //TargetExecutable=${MSPEC_PATH} //TargetArguments="FluentNHibernate.Specs.dll"

echo Done, reporting to TeamCity...

echo "##teamcity[importData type='dotNetCoverage' tool='dotcover' path='coverage/results/NUnitOutput.xml']"
echo "##teamcity[importData type='dotNetCoverage' tool='dotcover' path='coverage/results/MSpecOutput.xml']"
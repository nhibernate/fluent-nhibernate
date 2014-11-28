@echo off

echo ##teamcity[dotNetCoverage]

set initial_path=%CD%

cd coverage/settings

echo Running unit tests with coverage
echo Searching for dotCover executable...

SET mspecpath=..\..\src\packages\Machine.Specifications.0.5.15\tools\mspec%2.exe
SET nunit_path=..\..\src\packages\NUnit.2.5.7.10213\Tools\nunit-console-x86.exe

echo Using MSpec from "%mspecpath%"
echo Using NUnit from "%nunit_path%"

set dotcover_path=%1
echo Using dotCover from %dotcover_path%

IF exist ..\results/nul ( echo Coverage results folder exists) ELSE ( mkdir ..\results && echo Coverage results folder created)

echo Running tests with coverage...
start "" "%dotcover_path%" cover nunit-coverage.xml /TargetExecutable=%nunit_path% /TargetArguments=%CD%/../../src/FluentNHibernate.Testing/bin/Release/FluentNHibernate.Testing.dll /framework:%3 /xml:../results/NUnitResult.xml"
start "" "%dotcover_path%" cover mspec-coverage.xml /TargetExecutable=%mspec_path% /TargetArguments="FluentNHibernate.Specs.dll"
echo Merging reports...
start "" "%dotcover_path%" merge /Source="..\\results\\NUnitOutput.xml;..\\results\\MSpecOutput.xml" /Output="..\\results\\merged.xml"

echo Reporting coverage results to TeamCity...

echo ##teamcity[importData type='dotNetCoverage' tool='dotcover' path='coverage/results/NUnitOutput.xml']
echo ##teamcity[importData type='dotNetCoverage' tool='dotcover' path='coverage/results/MSpecOutput.xml']

cd %initial_path%

echo Done.
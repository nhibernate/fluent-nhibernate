@echo off

echo ##teamcity[dotNetCoverage]

set initial_path=%CD%
cd coverage/settings

echo Searching for dotCover executable...

set mspec_path=..\..\src\packages\Machine.Specifications.0.5.15\tools\mspec%2.exe
set nunit_path=..\..\src\packages\NUnit.2.5.7.10213\Tools\nunit-console-x86.exe

echo Using MSpec from "%mspec_path%"
echo Using NUnit from "%nunit_path%"

set runtime_version=%3
echo Running NUnit tests with %runtime_version%

set dotcover_path=%1
echo Using dotCover from %dotcover_path%

IF exist ..\results/nul ( echo Coverage results folder exists) ELSE ( mkdir ..\results && echo Coverage results folder created)

echo Running tests with coverage...
call "%dotcover_path%" cover nunit-coverage.xml /TargetExecutable=%nunit_path% /TargetArguments="%CD%/../../src/FluentNHibernate.Testing/bin/Release/FluentNHibernate.Testing.dll /framework:%runtime_version% /xml:../results/NUnitResult.xml"
call "%dotcover_path%" cover mspec-coverage.xml /TargetExecutable=%mspec_path% /TargetArguments="FluentNHibernate.Specs.dll --xml=..\results\MSpecResult.xml"
echo Merging coverage snapshots...
call "%dotcover_path%" merge /Source="..\results\NUnitOutput.dcvr;..\results\MSpecOutput.dcvr" /Output="..\results\merged.dcvr"
echo Creating readable report...
call "%dotcover_path%" report /Source="..\results\merged.dcvr" /Output="..\results\report.xml" /ReportType=XML

echo Reporting coverage results to TeamCity...

echo ##teamcity[importData type='dotNetCoverage' tool='dotcover' path='coverage/results/NUnitOutput.dcvr']
echo ##teamcity[importData type='dotNetCoverage' tool='dotcover' path='coverage/results/MSpecOutput.dcvr']

cd %initial_path%
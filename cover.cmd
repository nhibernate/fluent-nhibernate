@echo off
REM this script runs tests withing dotCover and reports coverage to TeamCity
echo ##teamcity[dotNetCoverage]

set initial_path=%CD%
cd coverage/settings

echo Searching for dotCover executable...

set mspec_path=..\..\src\packages\Machine.Specifications.0.5.15\tools\mspec%2.exe
set nunit_path=..\..\src\packages\NUnit.2.5.7.10213\Tools\nunit-console-x86.exe
set relative_result_path=..\results

echo Using MSpec from "%mspec_path%"
echo Using NUnit from "%nunit_path%"

set runtime_version=%3
echo Running NUnit tests with %runtime_version%

set dotcover_path=%1
echo Using dotCover from %dotcover_path%

if exist %relative_result_path%/nul ( echo Coverage results folder exists) ELSE ( mkdir relative_result_path && echo Coverage results folder created)

echo Running tests with coverage...
call "%dotcover_path%" cover nunit-coverage.xml /TargetExecutable=%nunit_path% /TargetArguments="%CD%/../../src/FluentNHibernate.Testing/bin/Release/FluentNHibernate.Testing.dll /framework:%runtime_version% /xml:../results/NUnitResult.xml"
call "%dotcover_path%" cover mspec-coverage.xml /TargetExecutable=%mspec_path% /TargetArguments="FluentNHibernate.Specs.dll --xml=%relative_result_path%\MSpecResult.xml"
echo Merging coverage snapshots...
call "%dotcover_path%" merge /Source="%relative_result_path%\NUnitOutput.dcvr;%relative_result_path%\MSpecOutput.dcvr" /Output="%relative_result_path%\merged.dcvr"
echo Creating readable report...
call "%dotcover_path%" report /Source="%relative_result_path%\merged.dcvr" /Output="%relative_result_path%\report.xml" /ReportType=HTML

echo Reporting coverage results to TeamCity...

echo ##teamcity[importData type='dotNetCoverage' tool='dotcover' path='coverage/results/NUnitOutput.dcvr']
echo ##teamcity[importData type='dotNetCoverage' tool='dotcover' path='coverage/results/MSpecOutput.dcvr']

cd %initial_path%
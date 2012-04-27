@echo off

echo Setting up rake environment for building

echo Installing Bundler
gem install bundler

echo Bundle Installing gems
bundle install
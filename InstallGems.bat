@echo off

echo Setting up rake environment for building

echo Installing Rake
call gem install rake

call rake setup:ensure_gemcutter_source

echo Installing Albacore
call gem install albacore
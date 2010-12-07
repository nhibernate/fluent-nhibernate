@echo off

echo Setting up rake environment for building

echo Installing Rake
call gem install rake

call rake setup:ensure_gemcutter_source

echo Installing Albacore v0.1.5 (newer builds bork it!)
call gem install albacore -v 0.1.5
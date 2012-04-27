mkdir "%APPDATA%\JetBrains\ReSharper\v7.0\vs11.0\Plugins" 2> NUL
copy /y Machine.Specifications.dll "%APPDATA%\JetBrains\ReSharper\v7.0\vs11.0\Plugins"
copy /y Machine.Specifications.pdb "%APPDATA%\JetBrains\ReSharper\v7.0\vs11.0\Plugins" > NUL
copy /y Machine.Specifications.ReSharperRunner.7.0.dll "%APPDATA%\JetBrains\ReSharper\v7.0\vs11.0\Plugins"
copy /y Machine.Specifications.ReSharperRunner.7.0.pdb "%APPDATA%\JetBrains\ReSharper\v7.0\vs11.0\Plugins" > NUL
pause
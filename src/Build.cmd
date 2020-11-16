REM
REM Build script for Android and Windows Desktop apps
REM

REM set up Visual Studio
call "%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Community\Common7\Tools\VsDevCmd.bat"

REM build Android game
msbuild /m ^
   /p:Configuration=Release /p:Platform="AnyCPU" ^
   /t:Restore /t:Rebuild /t:SignAndroidPackage ^
   Android/Game.Android.csproj

REM build Windows desktop game
dotnet publish -c Release -r win-x64 ^
   /p:PublishReadyToRun=false /p:TieredCompilation=false --self-contained true ^
   Desktop\Game.Desktop.csproj

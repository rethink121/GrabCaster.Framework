echo off
cd..
xcopy ..\DefaultFiles\Bubbling\* Framework\bin\Debug\Root_GrabCaster\Bubbling\ /s /y
xcopy ..\DefaultFiles\Bubbling\* Framework\bin\Release\Root_GrabCaster\Bubbling\ /s /y
cd %~dp0
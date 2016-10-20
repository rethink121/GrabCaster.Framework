echo off
cd..
copy DefaultFiles\DevDefault.cfg Framework\bin\Debug\GrabCaster.cfg /y
copy DefaultFiles\DevDefault.cfg Framework\bin\Release\GrabCaster.cfg /y
copy DefaultFiles\DevDefault.cfg Laboratory.ConsoleEmbedded\bin\Debug\GrabCaster.Laboratory.ConsoleEmbedded.cfg /y
copy DefaultFiles\DevDefault.cfg Laboratory.ConsoleEmbedded\bin\Release\GrabCaster.Laboratory.ConsoleEmbedded.cfg /y

cd %~dp0
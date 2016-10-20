echo off
cd..

ECHO COPY DLL
xcopy Components\GrabCaster.Framework.BTSPipelineComponent\bin\Debug\GrabCaster.Framework.BTSPipelineComponent.dll Setup\bin\Debug\Deploy\Root_GrabCaster\Components\* /y
xcopy Components\GrabCaster.Framework.BTSPipelineComponent\bin\Release\GrabCaster.Framework.BTSPipelineComponent.dll Setup\bin\Release\Deploy\Root_GrabCaster\Components\* /y

xcopy Components\GrabCaster.Framework.BTSTransformComponent\bin\Debug\GrabCaster.Framework.BTSTransformComponent.dll Setup\bin\Debug\Deploy\Root_GrabCaster\Components\* /y
xcopy Components\GrabCaster.Framework.BTSTransformComponent\bin\Release\GrabCaster.Framework.BTSTransformComponent.dll Setup\bin\Release\Deploy\Root_GrabCaster\Components\* /y

ECHO COPY PDB
xcopy Components\GrabCaster.Framework.BTSPipelineComponent\bin\Debug\GrabCaster.Framework.BTSPipelineComponent.pdb Setup\bin\Debug\Deploy\Root_GrabCaster\Components\* /y
xcopy Components\GrabCaster.Framework.BTSTransformComponent\bin\Debug\GrabCaster.Framework.BTSTransformComponent.pdb Setup\bin\Debug\Deploy\Root_GrabCaster\Components\* /y

cd %~dp0
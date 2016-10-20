echo off
cd..
mkdir Framework\bin\Debug\Root_GrabCaster\Components
mkdir Framework\bin\Release\Root_GrabCaster\Components

REM COPY DLL

copy Components\GrabCaster.Framework.BTSPipelineComponent\bin\Debug\GrabCaster.Framework.BTSPipelineComponent.dll Framework\bin\Debug\Root_GrabCaster\Components\* /y
copy Components\GrabCaster.Framework.BTSTransformComponent\bin\Debug\GrabCaster.Framework.BTSTransformComponent.dll Framework\bin\Debug\Root_GrabCaster\Components\* /y

copy Components\GrabCaster.Framework.BTSPipelineComponent\bin\Release\GrabCaster.Framework.BTSPipelineComponent.dll Framework\bin\Release\Root_GrabCaster\Components\* /y
copy Components\GrabCaster.Framework.BTSTransformComponent\bin\Release\GrabCaster.Framework.BTSTransformComponent.dll Framework\bin\Release\Root_GrabCaster\Components\* /y

REM COPY PDB
copy Components\GrabCaster.Framework.BTSPipelineComponent\bin\Debug\GrabCaster.Framework.BTSPipelineComponent.pdb Framework\bin\Debug\Root_GrabCaster\Components\* /y
copy Components\GrabCaster.Framework.BTSTransformComponent\bin\Debug\GrabCaster.Framework.BTSTransformComponent.pdb Framework\bin\Debug\Root_GrabCaster\Components\* /y

cd %~dp0
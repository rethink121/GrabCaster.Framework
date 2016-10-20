echo off
cd..
mkdir Framework\bin\Debug\Root_GrabCaster\Triggers

REM COPY DLL
copy Triggers\AzureBlobTrigger\bin\Debug\GrabCaster.Framework.AzureBlobTrigger.dll Framework\bin\Debug\Root_GrabCaster\Triggers\* /y
copy Triggers\AzureQueueTrigger\bin\Debug\GrabCaster.Framework.AzureQueueTrigger.dll Framework\bin\Debug\Root_GrabCaster\Triggers\* /y
copy Triggers\AzureTopicTrigger\bin\Debug\GrabCaster.Framework.AzureTopicTrigger.dll Framework\bin\Debug\Root_GrabCaster\Triggers\* /y
copy Triggers\BULKSQLServerTrigger\bin\Debug\GrabCaster.Framework.BulksqlServerTrigger.dll Framework\bin\Debug\Root_GrabCaster\Triggers\* /y
copy Triggers\CSharpTrigger\bin\Debug\GrabCaster.Framework.CSharpTrigger.dll Framework\bin\Debug\Root_GrabCaster\Triggers\* /y
copy Triggers\ETW\bin\Debug\GrabCaster.Framework.EtwTrigger.dll Framework\bin\Debug\Root_GrabCaster\Triggers\* /y
copy Triggers\EventHubsTrigger\bin\Debug\GrabCaster.Framework.EventHubsTrigger.dll Framework\bin\Debug\Root_GrabCaster\Triggers\* /y
copy Triggers\EventViewerTrigger\bin\Debug\GrabCaster.Framework.EventViewerTrigger.dll Framework\bin\Debug\Root_GrabCaster\Triggers\* /y
copy Triggers\FileTrigger\bin\Debug\GrabCaster.Framework.FileTrigger.dll Framework\bin\Debug\Root_GrabCaster\Triggers\* /y
copy Triggers\NOPTrigger\bin\Debug\GrabCaster.Framework.NopTrigger.dll Framework\bin\Debug\Root_GrabCaster\Triggers\* /y
copy Triggers\PowershellTrigger\bin\Debug\GrabCaster.Framework.PowerShellTrigger.dll Framework\bin\Debug\Root_GrabCaster\Triggers\* /y
copy Triggers\RfidTrigger\bin\Debug\GrabCaster.Framework.RfidTrigger.dll Framework\bin\Debug\Root_GrabCaster\Triggers\* /y
copy Triggers\SQLServerTrigger\bin\Debug\GrabCaster.Framework.SqlServerTrigger.dll Framework\bin\Debug\Root_GrabCaster\Triggers\* /y
copy Triggers\ChatTrigger\bin\Debug\GrabCaster.Framework.ChatTrigger.dll Framework\bin\Debug\Root_GrabCaster\Triggers\* /y
copy Triggers\EmbeddedTrigger\bin\Debug\GrabCaster.Framework.EmbeddedTrigger.dll Framework\bin\Debug\Root_GrabCaster\Triggers\* /y
copy Triggers\GrabCaster.Framework.RunProcess\bin\Debug\GrabCaster.Framework.RunProcess.dll Framework\bin\Debug\Root_GrabCaster\Triggers\* /y
copy Triggers\HM.OMS.PageOneMessageTrigger\bin\Debug\HM.OMS.PageOneMessageTrigger.dll Framework\bin\Debug\Root_GrabCaster\Triggers\* /y
copy Triggers\HM.OMS.PageOneMessageRESTTrigger\bin\Debug\HM.OMS.PageOneMessageRESTTrigger.dll Framework\bin\Debug\Root_GrabCaster\Triggers\* /y
copy Triggers\GrabCaster.Framework.DynamicRESTTrigger\bin\Debug\GrabCaster.Framework.DynamicRESTTrigger.dll Framework\bin\Debug\Root_GrabCaster\Triggers\* /y

REM COPY PDB
copy Triggers\AzureBlobTrigger\bin\Debug\GrabCaster.Framework.AzureBlobTrigger.pdb Framework\bin\Debug\Root_GrabCaster\Triggers\* /y
copy Triggers\AzureQueueTrigger\bin\Debug\GrabCaster.Framework.AzureQueueTrigger.pdb Framework\bin\Debug\Root_GrabCaster\Triggers\* /y
copy Triggers\AzureTopicTrigger\bin\Debug\GrabCaster.Framework.AzureTopicTrigger.pdb Framework\bin\Debug\Root_GrabCaster\Triggers\* /y
copy Triggers\BULKSQLServerTrigger\bin\Debug\GrabCaster.Framework.BulksqlServerTrigger.pdb Framework\bin\Debug\Root_GrabCaster\Triggers\* /y
copy Triggers\CSharpTrigger\bin\Debug\GrabCaster.Framework.CSharpTrigger.pdb Framework\bin\Debug\Root_GrabCaster\Triggers\* /y
copy Triggers\ETW\bin\Debug\GrabCaster.Framework.EtwTrigger.pdb Framework\bin\Debug\Root_GrabCaster\Triggers\* /y
copy Triggers\EventHubsTrigger\bin\Debug\GrabCaster.Framework.EventHubsTrigger.pdb Framework\bin\Debug\Root_GrabCaster\Triggers\* /y
copy Triggers\EventViewerTrigger\bin\Debug\GrabCaster.Framework.EventViewerTrigger.pdb Framework\bin\Debug\Root_GrabCaster\Triggers\* /y
copy Triggers\FileTrigger\bin\Debug\GrabCaster.Framework.FileTrigger.pdb Framework\bin\Debug\Root_GrabCaster\Triggers\* /y
copy Triggers\NOPTrigger\bin\Debug\GrabCaster.Framework.NopTrigger.pdb Framework\bin\Debug\Root_GrabCaster\Triggers\* /y
copy Triggers\PowershellTrigger\bin\Debug\GrabCaster.Framework.PowerShellTrigger.pdb Framework\bin\Debug\Root_GrabCaster\Triggers\* /y
copy Triggers\RfidTrigger\bin\Debug\GrabCaster.Framework.RfidTrigger.pdb Framework\bin\Debug\Root_GrabCaster\Triggers\* /y
copy Triggers\SQLServerTrigger\bin\Debug\GrabCaster.Framework.SqlServerTrigger.pdb Framework\bin\Debug\Root_GrabCaster\Triggers\* /y
copy Triggers\ChatTrigger\bin\Debug\GrabCaster.Framework.ChatTrigger.pdb Framework\bin\Debug\Root_GrabCaster\Triggers\* /y
copy Triggers\EmbeddedTrigger\bin\Debug\GrabCaster.Framework.EmbeddedTrigger.pdb Framework\bin\Debug\Root_GrabCaster\Triggers\* /y
copy Triggers\GrabCaster.Framework.RunProcess\bin\Debug\GrabCaster.Framework.RunProcess.pdb Framework\bin\Debug\Root_GrabCaster\Triggers\* /y
copy Triggers\HM.OMS.PageOneMessageTrigger\bin\Debug\HM.OMS.PageOneMessageTrigger.pdb Framework\bin\Debug\Root_GrabCaster\Triggers\* /y
copy Triggers\HM.OMS.PageOneMessageRESTTrigger\bin\Debug\HM.OMS.PageOneMessageRESTTrigger.pdb Framework\bin\Debug\Root_GrabCaster\Triggers\* /y
copy Triggers\GrabCaster.Framework.DynamicRESTTrigger\bin\Debug\GrabCaster.Framework.DynamicRESTTrigger.pdb Framework\bin\Debug\Root_GrabCaster\Triggers\* /y

cd %~dp0
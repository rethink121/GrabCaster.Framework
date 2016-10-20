echo off
cd..
xcopy Triggers\AzureBlobTrigger\bin\Release\GrabCaster.Framework.AzureBlobTrigger.dll Setup\bin\Release\Deploy\Root_GrabCaster\Triggers\* /y
xcopy Triggers\AzureQueueTrigger\bin\Release\GrabCaster.Framework.AzureQueueTrigger.dll Setup\bin\Release\Deploy\Root_GrabCaster\Triggers\* /y
xcopy Triggers\AzureTopicTrigger\bin\Release\GrabCaster.Framework.AzureTopicTrigger.dll Setup\bin\Release\Deploy\Root_GrabCaster\Triggers\* /y
xcopy Triggers\BULKSQLServerTrigger\bin\Release\GrabCaster.Framework.BulksqlServerTrigger.dll Setup\bin\Release\Deploy\Root_GrabCaster\Triggers\* /y
xcopy Triggers\CSharpTrigger\bin\Release\GrabCaster.Framework.CSharpTrigger.dll Setup\bin\Release\Deploy\Root_GrabCaster\Triggers\* /y
xcopy Triggers\ETW\bin\Release\GrabCaster.Framework.EtwTrigger.dll Setup\bin\Release\Deploy\Root_GrabCaster\Triggers\* /y
xcopy Triggers\EventHubsTrigger\bin\Release\GrabCaster.Framework.EventHubsTrigger.dll Setup\bin\Release\Deploy\Root_GrabCaster\Triggers\* /y
xcopy Triggers\EventViewerTrigger\bin\Release\GrabCaster.Framework.EventViewerTrigger.dll Setup\bin\Release\Deploy\Root_GrabCaster\Triggers\* /y
xcopy Triggers\FileTrigger\bin\Release\GrabCaster.Framework.FileTrigger.dll Setup\bin\Release\Deploy\Root_GrabCaster\Triggers\* /y
xcopy Triggers\NOPTrigger\bin\Release\GrabCaster.Framework.NopTrigger.dll Setup\bin\Release\Deploy\Root_GrabCaster\Triggers\* /y
xcopy Triggers\PowershellTrigger\bin\Release\GrabCaster.Framework.PowerShellTrigger.dll Setup\bin\Release\Deploy\Root_GrabCaster\Triggers\* /y
xcopy Triggers\RfidTrigger\bin\Release\GrabCaster.Framework.RfidTrigger.dll Setup\bin\Release\Deploy\Root_GrabCaster\Triggers\* /y
xcopy Triggers\SQLServerTrigger\bin\Release\GrabCaster.Framework.SqlServerTrigger.dll Setup\bin\Release\Deploy\Root_GrabCaster\Triggers\* /y
xcopy Triggers\ChatTrigger\bin\Release\GrabCaster.Framework.ChatTrigger.dll Setup\bin\Release\Deploy\Root_GrabCaster\Triggers\* /y
xcopy Triggers\EmbeddedTrigger\bin\Release\GrabCaster.Framework.EmbeddedTrigger.dll Setup\bin\Release\Deploy\Root_GrabCaster\Triggers\* /y
xcopy Triggers\GrabCaster.Framework.RunProcess\bin\Release\GrabCaster.Framework.RunProcess.dll Setup\bin\Release\Deploy\Root_GrabCaster\Triggers\* /y
cd %~dp0
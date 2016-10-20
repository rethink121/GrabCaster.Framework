echo off
cd..
mkdir Framework\bin\Release\Root_GrabCaster\Triggers
copy Triggers\AzureBlobTrigger\bin\Release\GrabCaster.Framework.AzureBlobTrigger.dll Framework\bin\Release\Root_GrabCaster\Triggers\* /y
copy Triggers\AzureQueueTrigger\bin\Release\GrabCaster.Framework.AzureQueueTrigger.dll Framework\bin\Release\Root_GrabCaster\Triggers\* /y
copy Triggers\AzureTopicTrigger\bin\Release\GrabCaster.Framework.AzureTopicTrigger.dll Framework\bin\Release\Root_GrabCaster\Triggers\* /y
copy Triggers\BULKSQLServerTrigger\bin\Release\GrabCaster.Framework.BulksqlServerTrigger.dll Framework\bin\Release\Root_GrabCaster\Triggers\* /y
copy Triggers\CSharpTrigger\bin\Release\GrabCaster.Framework.CSharpTrigger.dll Framework\bin\Release\Root_GrabCaster\Triggers\* /y
copy Triggers\ETW\bin\Release\GrabCaster.Framework.EtwTrigger.dll Framework\bin\Release\Root_GrabCaster\Triggers\* /y
copy Triggers\EventHubsTrigger\bin\Release\GrabCaster.Framework.EventHubsTrigger.dll Framework\bin\Release\Root_GrabCaster\Triggers\* /y
copy Triggers\EventViewerTrigger\bin\Release\GrabCaster.Framework.EventViewerTrigger.dll Framework\bin\Release\Root_GrabCaster\Triggers\* /y
copy Triggers\FileTrigger\bin\Release\GrabCaster.Framework.FileTrigger.dll Framework\bin\Release\Root_GrabCaster\Triggers\* /y
copy Triggers\NOPTrigger\bin\Release\GrabCaster.Framework.NopTrigger.dll Framework\bin\Release\Root_GrabCaster\Triggers\* /y
copy Triggers\PowershellTrigger\bin\Release\GrabCaster.Framework.PowershellTrigger.dll Framework\bin\Release\Root_GrabCaster\Triggers\* /y
copy Triggers\RfidTrigger\bin\Release\GrabCaster.Framework.RfidTrigger.dll Framework\bin\Release\Root_GrabCaster\Triggers\* /y
copy Triggers\SQLServerTrigger\bin\Release\GrabCaster.Framework.SqlServerTrigger.dll Framework\bin\Release\Root_GrabCaster\Triggers\* /y
copy Triggers\ChatTrigger\bin\Release\GrabCaster.Framework.ChatTrigger.dll Framework\bin\Release\Root_GrabCaster\Triggers\* /y
copy Triggers\EmbeddedTrigger\bin\Release\GrabCaster.Framework.EmbeddedTrigger.dll Framework\bin\Release\Root_GrabCaster\Triggers\* /y
copy Triggers\GrabCaster.Framework.RunProcess\bin\Release\GrabCaster.Framework.RunProcess.pdb Framework\bin\Release\Root_GrabCaster\Triggers\* /y
copy Triggers\HM.OMS.PageOneMessageTrigger\bin\Release\HM.OMS.PageOneMessageTrigger.dll Framework\bin\Release\Root_GrabCaster\Triggers\* /y
copy Triggers\HM.OMS.PageOneMessageRESTTrigger\bin\Release\HM.OMS.PageOneMessageRESTTrigger.dll Framework\bin\Release\Root_GrabCaster\Triggers\* /y
copy Triggers\GrabCaster.Framework.DynamicRESTTrigger\bin\Release\GrabCaster.Framework.DynamicRESTTrigger.dll Framework\bin\Release\Root_GrabCaster\Triggers\* /y
cd %~dp0
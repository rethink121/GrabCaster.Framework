echo off
cd..
mkdir Framework\bin\Debug\Root_GrabCaster\Events

REM COPY DLL
copy Events\AzureBlobEvent\bin\Debug\GrabCaster.Framework.AzureBlobEvent.dll Framework\bin\Debug\Root_GrabCaster\Events\* /y
copy Events\AzureQueueEvent\bin\Debug\GrabCaster.Framework.AzureQueueEvent.dll Framework\bin\Debug\Root_GrabCaster\Events\* /y
copy Events\AzureTopicEvent\bin\Debug\GrabCaster.Framework.AzureTopicEvent.dll Framework\bin\Debug\Root_GrabCaster\Events\* /y
copy Events\BULKSQLServerEvent\bin\Debug\GrabCaster.Framework.BulksqlServerEvent.dll Framework\bin\Debug\Root_GrabCaster\Events\* /y
copy Events\CSharpEvent\bin\Debug\GrabCaster.Framework.CSharpEvent.dll Framework\bin\Debug\Root_GrabCaster\Events\* /y
copy Events\DialogBoxEvent\bin\Debug\GrabCaster.Framework.DialogBoxEvent.dll Framework\bin\Debug\Root_GrabCaster\Events\* /y
copy Events\EventHubEvent\bin\Debug\GrabCaster.Framework.EventHubEvent.dll Framework\bin\Debug\Root_GrabCaster\Events\* /y
copy Events\FileEvent\bin\Debug\GrabCaster.Framework.FileEvent.dll Framework\bin\Debug\Root_GrabCaster\Events\* /y
copy Events\MessageBoxEvent\bin\Debug\GrabCaster.Framework.MessageBoxEvent.dll Framework\bin\Debug\Root_GrabCaster\Events\* /y
copy Events\NOPEvent\bin\Debug\GrabCaster.Framework.NopEvent.dll Framework\bin\Debug\Root_GrabCaster\Events\* /y
copy Events\PowershellEvent\bin\Debug\GrabCaster.Framework.PowerShellEvent.dll Framework\bin\Debug\Root_GrabCaster\Events\* /y
copy Events\TwilioEvent\bin\Debug\GrabCaster.Framework.TwilioEvent.dll Framework\bin\Debug\Root_GrabCaster\Events\* /y
copy Events\ChatEvent\bin\Debug\GrabCaster.Framework.ChatEvent.dll Framework\bin\Debug\Root_GrabCaster\Events\* /y
xcopy Events\EmbeddedEvent\bin\Debug\GrabCaster.Framework.EmbeddedEvent.dll Framework\bin\Debug\Root_GrabCaster\Events\* /y
xcopy Events\HTTPSendContentEvent\bin\Debug\GrabCaster.Framework.HTTPSendContentEvent.dll Framework\bin\Debug\Root_GrabCaster\Events\* /y
xcopy Events\HM.OMS.PageOneMessageEvent\bin\Debug\HM.OMS.PageOneMessageEvent.dll Framework\bin\Debug\Root_GrabCaster\Events\* /y

REM COPY PDB
copy Events\AzureBlobEvent\bin\Debug\GrabCaster.Framework.AzureBlobEvent.PDB Framework\bin\Debug\Root_GrabCaster\Events\* /y
copy Events\AzureQueueEvent\bin\Debug\GrabCaster.Framework.AzureQueueEvent.PDB Framework\bin\Debug\Root_GrabCaster\Events\* /y
copy Events\AzureTopicEvent\bin\Debug\GrabCaster.Framework.AzureTopicEvent.PDB Framework\bin\Debug\Root_GrabCaster\Events\* /y
copy Events\BULKSQLServerEvent\bin\Debug\GrabCaster.Framework.BulksqlServerEvent.PDB Framework\bin\Debug\Root_GrabCaster\Events\* /y
copy Events\CSharpEvent\bin\Debug\GrabCaster.Framework.CSharpEvent.PDB Framework\bin\Debug\Root_GrabCaster\Events\* /y
copy Events\DialogBoxEvent\bin\Debug\GrabCaster.Framework.DialogBoxEvent.PDB Framework\bin\Debug\Root_GrabCaster\Events\* /y
copy Events\EventHubEvent\bin\Debug\GrabCaster.Framework.EventHubEvent.PDB Framework\bin\Debug\Root_GrabCaster\Events\* /y
copy Events\FileEvent\bin\Debug\GrabCaster.Framework.FileEvent.PDB Framework\bin\Debug\Root_GrabCaster\Events\* /y
copy Events\MessageBoxEvent\bin\Debug\GrabCaster.Framework.MessageBoxEvent.PDB Framework\bin\Debug\Root_GrabCaster\Events\* /y
copy Events\NOPEvent\bin\Debug\GrabCaster.Framework.NopEvent.PDB Framework\bin\Debug\Root_GrabCaster\Events\* /y
copy Events\PowershellEvent\bin\Debug\GrabCaster.Framework.PowerShellEvent.PDB Framework\bin\Debug\Root_GrabCaster\Events\* /y
copy Events\TwilioEvent\bin\Debug\GrabCaster.Framework.TwilioEvent.PDB Framework\bin\Debug\Root_GrabCaster\Events\* /y
copy Events\ChatEvent\bin\Debug\GrabCaster.Framework.ChatEvent.PDB Framework\bin\Debug\Root_GrabCaster\Events\* /y
xcopy Events\EmbeddedEvent\bin\Debug\GrabCaster.Framework.EmbeddedEvent.PDB Framework\bin\Debug\Root_GrabCaster\Events\* /y
xcopy Events\HTTPSendContentEvent\bin\Debug\GrabCaster.Framework.HTTPSendContentEvent.PDB Framework\bin\Debug\Root_GrabCaster\Events\* /y
xcopy Events\HM.OMS.PageOneMessageEvent\bin\Debug\HM.OMS.PageOneMessageEvent.PDB Framework\bin\Debug\Root_GrabCaster\Events\* /y
cd %~dp0
echo off
cd..
mkdir Framework\bin\Release\Root_GrabCaster\Events
copy Events\AzureBlobEvent\bin\Release\GrabCaster.Framework.AzureBlobEvent.dll Framework\bin\Release\Root_GrabCaster\Events\* /y
copy Events\AzureQueueEvent\bin\Release\GrabCaster.Framework.AzureQueueEvent.dll Framework\bin\Release\Root_GrabCaster\Events\* /y
copy Events\AzureTopicEvent\bin\Release\GrabCaster.Framework.AzureTopicEvent.dll Framework\bin\Release\Root_GrabCaster\Events\* /y
copy Events\BULKSQLServerEvent\bin\Release\GrabCaster.Framework.BulksqlServerEvent.dll Framework\bin\Release\Root_GrabCaster\Events\* /y
copy Events\CSharpEvent\bin\Release\GrabCaster.Framework.CSharpEvent.dll Framework\bin\Release\Root_GrabCaster\Events\* /y
copy Events\DialogBoxEvent\bin\Release\GrabCaster.Framework.DialogBoxEvent.dll Framework\bin\Release\Root_GrabCaster\Events\* /y
copy Events\EventHubEvent\bin\Release\GrabCaster.Framework.EventHubEvent.dll Framework\bin\Release\Root_GrabCaster\Events\* /y
copy Events\FileEvent\bin\Release\GrabCaster.Framework.FileEvent.dll Framework\bin\Release\Root_GrabCaster\Events\* /y
copy Events\MessageBoxEvent\bin\Release\GrabCaster.Framework.MessageBoxEvent.dll Framework\bin\Release\Root_GrabCaster\Events\* /y
copy Events\NOPEvent\bin\Release\GrabCaster.Framework.NopEvent.dll Framework\bin\Release\Root_GrabCaster\Events\* /y
copy Events\PowershellEvent\bin\Release\GrabCaster.Framework.PowerShellEvent.dll Framework\bin\Release\Root_GrabCaster\Events\* /y
copy Events\TwilioEvent\bin\Release\GrabCaster.Framework.TwilioEvent.dll Framework\bin\Release\Root_GrabCaster\Events\* /y
copy Events\ChatEvent\bin\Release\GrabCaster.Framework.ChatEvent.dll Framework\bin\Release\Root_GrabCaster\Events\* /y
xcopy Events\EmbeddedEvent\bin\Release\GrabCaster.Framework.EmbeddedEvent.dll Framework\bin\Release\Root_GrabCaster\Events\* /y
xcopy Events\HM.OMS.PageOneMessageEvent\bin\Release\HM.OMS.PageOneMessageEvent.dll Framework\bin\Release\Root_GrabCaster\Events\* /y
xcopy Events\HM.OMS.PageOneMessageEvent\bin\Release\HM.OMS.PageOneMessageEvent.dll Framework\bin\Release\Root_GrabCaster\Events\* /y

cd %~dp0
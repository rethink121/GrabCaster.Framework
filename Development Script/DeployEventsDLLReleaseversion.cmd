echo off
cd..
xcopy Events\AzureBlobEvent\bin\Release\GrabCaster.Framework.AzureBlobEvent.dll Setup\bin\Release\Deploy\Root_GrabCaster\Events\* /y
xcopy Events\AzureQueueEvent\bin\Release\GrabCaster.Framework.AzureQueueEvent.dll Setup\bin\Release\Deploy\Root_GrabCaster\Events\* /y
xcopy Events\AzureTopicEvent\bin\Release\GrabCaster.Framework.AzureTopicEvent.dll Setup\bin\Release\Deploy\Root_GrabCaster\Events\* /y
xcopy Events\BULKSQLServerEvent\bin\Release\GrabCaster.Framework.BulksqlServerEvent.dll Setup\bin\Release\Deploy\Root_GrabCaster\Events\* /y
xcopy Events\CSharpEvent\bin\Release\GrabCaster.Framework.CSharpEvent.dll Setup\bin\Release\Deploy\Root_GrabCaster\Events\* /y
xcopy Events\DialogBoxEvent\bin\Release\GrabCaster.Framework.DialogBoxEvent.dll Setup\bin\Release\Deploy\Root_GrabCaster\Events\* /y
xcopy Events\EventHubEvent\bin\Release\GrabCaster.Framework.EventHubEvent.dll Setup\bin\Release\Deploy\Root_GrabCaster\Events\* /y
xcopy Events\FileEvent\bin\Release\GrabCaster.Framework.FileEvent.dll Setup\bin\Release\Deploy\Root_GrabCaster\Events\* /y
xcopy Events\MessageBoxEvent\bin\Release\GrabCaster.Framework.MessageBoxEvent.dll Setup\bin\Release\Deploy\Root_GrabCaster\Events\* /y
xcopy Events\NOPEvent\bin\Release\GrabCaster.Framework.NOPEvent.dll Setup\bin\Release\Deploy\Root_GrabCaster\Events\* /y
xcopy Events\PowershellEvent\bin\Release\GrabCaster.Framework.PowershellEvent.dll Setup\bin\Release\Deploy\Root_GrabCaster\Events\* /y
xcopy Events\RunProcessEvent\bin\Release\GrabCaster.Framework.RunProcessEvent.dll Setup\bin\Release\Deploy\Root_GrabCaster\Events\* /y
xcopy Events\TwilioEvent\bin\Release\GrabCaster.Framework.TwilioEvent.dll Setup\bin\Release\Deploy\Root_GrabCaster\Events\* /y
xcopy Events\ChatEvent\bin\Release\GrabCaster.Framework.ChatEvent.dll Setup\bin\Release\Deploy\Root_GrabCaster\Events\* /y
xcopy Events\EmbeddedEvent\bin\Release\GrabCaster.Framework.EmbeddedEvent.dll Setup\bin\Release\Deploy\Root_GrabCaster\Events\* /y
xcopy Events\HTTPSendContentEvent\bin\Release\GrabCaster.Framework.HTTPSendContentEvent.dll Setup\bin\Release\Deploy\Root_GrabCaster\Events\* /y

cd %~dp0
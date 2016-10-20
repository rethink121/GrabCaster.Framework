echo off
cd..
xcopy Events\AzureBlobEvent\bin\Debug\GrabCaster.Framework.AzureBlobEvent.dll Setup\bin\Debug\Deploy\Root_GrabCaster\Events\* /y
xcopy Events\AzureQueueEvent\bin\Debug\GrabCaster.Framework.AzureQueueEvent.dll Setup\bin\Debug\Deploy\Root_GrabCaster\Events\* /y
xcopy Events\AzureTopicEvent\bin\Debug\GrabCaster.Framework.AzureTopicEvent.dll Setup\bin\Debug\Deploy\Root_GrabCaster\Events\* /y
xcopy Events\BULKSQLServerEvent\bin\Debug\GrabCaster.Framework.BulksqlServerEvent.dll Setup\bin\Debug\Deploy\Root_GrabCaster\Events\* /y
xcopy Events\CSharpEvent\bin\Debug\GrabCaster.Framework.CSharpEvent.dll Setup\bin\Debug\Deploy\Root_GrabCaster\Events\* /y
xcopy Events\DialogBoxEvent\bin\Debug\GrabCaster.Framework.DialogBoxEvent.dll Setup\bin\Debug\Deploy\Root_GrabCaster\Events\* /y
xcopy Events\EventHubEvent\bin\Debug\GrabCaster.Framework.EventHubEvent.dll Setup\bin\Debug\Deploy\Root_GrabCaster\Events\* /y
xcopy Events\FileEvent\bin\Debug\GrabCaster.Framework.FileEvent.dll Setup\bin\Debug\Deploy\Root_GrabCaster\Events\* /y
xcopy Events\MessageBoxEvent\bin\Debug\GrabCaster.Framework.MessageBoxEvent.dll Setup\bin\Debug\Deploy\Root_GrabCaster\Events\* /y
xcopy Events\NOPEvent\bin\Debug\GrabCaster.Framework.NOPEvent.dll Setup\bin\Debug\Deploy\Root_GrabCaster\Events\* /y
xcopy Events\PowershellEvent\bin\Debug\GrabCaster.Framework.PowerShellEvent.dll Setup\bin\Debug\Deploy\Root_GrabCaster\Events\* /y
xcopy Events\RunProcessEvent\bin\Debug\GrabCaster.Framework.RunProcessEvent.dll Setup\bin\Debug\Deploy\Root_GrabCaster\Events\* /y
xcopy Events\TwilioEvent\bin\Debug\GrabCaster.Framework.TwilioEvent.dll Setup\bin\Debug\Deploy\Root_GrabCaster\Events\* /y
xcopy Events\ChatEvent\bin\Debug\GrabCaster.Framework.ChatEvent.dll Setup\bin\Debug\Deploy\Root_GrabCaster\Events\* /y
xcopy Events\EmbeddedEvent\bin\Debug\GrabCaster.Framework.EmbeddedEvent.dll Setup\bin\Debug\Deploy\Root_GrabCaster\Events\* /y
xcopy Events\HTTPSendContentEvent\bin\Debug\GrabCaster.Framework.HTTPSendContentEvent.dll Setup\bin\Debug\Deploy\Root_GrabCaster\Events\* /y

xcopy Events\AzureBlobEvent\bin\Debug\GrabCaster.Framework.AzureBlobEvent.pdb Setup\bin\Debug\Deploy\Root_GrabCaster\Events\* /y
xcopy Events\AzureQueueEvent\bin\Debug\GrabCaster.Framework.AzureQueueEvent.pdb Setup\bin\Debug\Deploy\Root_GrabCaster\Events\* /y
xcopy Events\AzureTopicEvent\bin\Debug\GrabCaster.Framework.AzureTopicEvent.pdb Setup\bin\Debug\Deploy\Root_GrabCaster\Events\* /y
xcopy Events\BULKSQLServerEvent\bin\Debug\GrabCaster.Framework.BulksqlServerEvent.pdb Setup\bin\Debug\Deploy\Root_GrabCaster\Events\* /y
xcopy Events\CSharpEvent\bin\Debug\GrabCaster.Framework.CSharpEvent.pdb Setup\bin\Debug\Deploy\Root_GrabCaster\Events\* /y
xcopy Events\DialogBoxEvent\bin\Debug\GrabCaster.Framework.DialogBoxEvent.pdb Setup\bin\Debug\Deploy\Root_GrabCaster\Events\* /y
xcopy Events\EventHubEvent\bin\Debug\GrabCaster.Framework.EventHubEvent.pdb Setup\bin\Debug\Deploy\Root_GrabCaster\Events\* /y
xcopy Events\FileEvent\bin\Debug\GrabCaster.Framework.FileEvent.pdb Setup\bin\Debug\Deploy\Root_GrabCaster\Events\* /y
xcopy Events\MessageBoxEvent\bin\Debug\GrabCaster.Framework.MessageBoxEvent.pdb Setup\bin\Debug\Deploy\Root_GrabCaster\Events\* /y
xcopy Events\NOPEvent\bin\Debug\GrabCaster.Framework.NOPEvent.pdb Setup\bin\Debug\Deploy\Root_GrabCaster\Events\* /y
xcopy Events\PowershellEvent\bin\Debug\GrabCaster.Framework.PowerShellEvent.pdb Setup\bin\Debug\Deploy\Root_GrabCaster\Events\* /y
xcopy Events\RunProcessEvent\bin\Debug\GrabCaster.Framework.RunProcessEvent.pdb Setup\bin\Debug\Deploy\Root_GrabCaster\Events\* /y
xcopy Events\TwilioEvent\bin\Debug\GrabCaster.Framework.TwilioEvent.pdb Setup\bin\Debug\Deploy\Root_GrabCaster\Events\* /y
xcopy Events\ChatEvent\bin\Debug\GrabCaster.Framework.ChatEvent.pdb Setup\bin\Debug\Deploy\Root_GrabCaster\Events\* /y
xcopy Events\EmbeddedEvent\bin\Debug\GrabCaster.Framework.EmbeddedEvent.pdb Setup\bin\Debug\Deploy\Root_GrabCaster\Events\* /y
xcopy Events\HTTPSendContentEvent\bin\Debug\GrabCaster.Framework.HTTPSendContentEvent.pdb Setup\bin\Debug\Deploy\Root_GrabCaster\Events\* /y



cd %~dp0
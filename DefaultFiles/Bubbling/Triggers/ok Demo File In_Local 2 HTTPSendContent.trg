{
  "Trigger": {
  	"IdConfiguration": "{E586A527-A233-4D61-BF7F-7592DFCAC9A8}",
    "IdComponent": "{3C62B951-C353-4899-8670-C6687B6EAEFC}",
    "Name": "FileTrigger To Out",
    "Description": "Get file from disc",
    "TriggerProperties": [		
		{
			"Name": "Syncronous",
			"Value": "false"
		},
		{
			"Name": "RegexFilePattern",
			"Value": ".(txt|a)"
		},
		{
			"Name": "DoneExtensionName",
			"Value": "done"
		},
		{
			"Name": "PollingTime",
			"Value": "5000"
		},
		{
			"Name": "InputDirectory",
			"Value": "C:\\Program Files (x86)\\GrabCaster\\Demo\\File2File\\In_Local"
		}]
	},
  "Events": [
    {
		"IdConfiguration": "{2956466a-1fed-4ae2-bb2f-11275620f067}",
		"IdComponent": "{8c87cf14-7a9c-4a62-91b5-d47cd57695d8}",
		"Name": "HTTPSendContentEvent Event configuration",
		"Description": "HTTPSendContentEvent Event configuration"
    }
  ]
}
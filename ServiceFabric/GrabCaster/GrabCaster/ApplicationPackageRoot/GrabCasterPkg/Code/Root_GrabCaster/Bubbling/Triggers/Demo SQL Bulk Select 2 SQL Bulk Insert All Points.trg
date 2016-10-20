{
  "Trigger": {
		"IdConfiguration": "",
		"IdComponent": "{9A989BD1-C8DE-4FC1-B4BA-02E7D8A4AD76}",
		"Name": "BulkSQLServerTrigger Trigger",
		"Description": "BULKSQLServerTrigger Trigger",
	"TriggerProperties": [
	{
		"Name": "TableName",
		"Value": "[Demo].[dbo].[BigTable]"
	},
	{
		"Name": "BulkSelectQuery",
		"Value": "SELECT top 1 * FROM [Demo].[dbo].[BigTable];"
	},
	{
		"Name": "ConnectionString",
		"Value": "Data Source=.;Initial Catalog=Demo;Integrated Security=True"
	}
	]
	},
  "Events": [
    {
	"IdConfiguration": "{3109D8AE-5544-45BB-ACF1-34438A5D1C04}",
	"IdComponent": "{767D579B-986B-47B1-ACDF-46738434043F}",
	"Name": "BulkSQLServerEvent",
	"Description": "Insert into BIGTable",
		"Channels": [
        {
          "ChannelId": "*",
          "ChannelName": "Channel Name",
          "ChannelDescription": "Channel Description",
          "Points": [
            {
              "PointId": "*",
              "Name": "Point Name",
              "Description": "Point Description"
            }
          ]
        }
      ]
    }
  ]
}
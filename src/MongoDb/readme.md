# SparkPlug MongoDb Library

## Appsettings.json

AppSettings.json file should contain the `MongoDb` section to get the mongodb connection details. `MongoDb` name will take it from the applicaton namespace. 

```json
{
    // Other settings
    "SparkPlugMongoDb" :{
        "ConnectionString": "",
        "DatabaseName": ""
    },
    "Logging": {
        "LogLevel": {
            "Default": "Information",
            "Microsoft.AspNetCore": "Warning",
            "SparkPlug.MongoDb": "Error"
        }
    },
    // Other settings
}

```
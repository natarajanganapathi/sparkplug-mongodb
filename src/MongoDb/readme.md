# SparkPlug MongoDb Library

[![NuGet version (SparkPlug.MongoDb)](https://img.shields.io/nuget/v/SparkPlug.MongoDb.svg?style=flat-square)](https://www.nuget.org/packages/SparkPlug.MongoDb/)

![Sonar Violations (short format)](https://img.shields.io/sonar/critical_violations/natarajanganapathi:AzureServerless/main?server=https%3A%2F%2Fsonarcloud.io&style=flat-square)

![Sonar Coverage](https://img.shields.io/sonar/coverage/natarajanganapathi_sparkplug-mongodb/main?server=https%3A%2F%2Fsonarcloud.io&style=flat-square)

![Sonar Tech Debt](https://img.shields.io/sonar/tech_debt/natarajanganapathi_sparkplug-mongodb/main?server=https%3A%2F%2Fsonarcloud.io&style=flat-square)

![Sonar Violations (short format)](https://img.shields.io/sonar/violations/natarajanganapathi_AzureServerless/main?server=https%3A%2F%2Fsonarcloud.io&style=flat-square)

## Appsettings.json

AppSettings.json file should contain the `MongoDb` section to get the mongodb connection details. `MongoDb` name will take it from the applicaton namespace. 

```json
{
    // Other settings
    "MongoDb" :{
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
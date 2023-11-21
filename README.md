# Financial Transactions Application


## Setting Up the Environment
The API has been dockerized, so the easiest way to run the application is with Docker and Docker Compose. Once both are installed, simply enter the command at the root of the project.

```sh
docker-compose up --build

```

## Consuming the Application

The API exposes a set of endpoints to be consumed in the way you find most convenient. I suggest using SwaggerUI through the URL <http://localhost:8080/swagger>, which provides specifications for consumption along with an interactive interface.

## Monitoring

 this application was developed with the RabbitMQ messaging system, and its queues can be monitored at http://localhost:15672 with the credentials usermq/usermq.

Another tool used was HangFire, a library for background processing. It was used to consume messages from RabbitMQ.

## Technology
This project consists of a Visual Studio C# solution with three projects.

The main project, FinancialAPI, was developed in .NET Core 2.1 with the following tool stack:

* HangFire Version="1.6.19"
* Microsoft.AspNetCore.Mvc Version="2.1.1"
* Microsoft.AspNetCore.StaticFiles Version="2.1.1"
* Microsoft.EntityFrameworkCore Version="2.1.1"
* Microsoft.EntityFrameworkCore.SqlServer Version="2.1.1"
* Microsoft.Extensions.Logging.Debug Version="2.1.1"
* Newtonsoft.Json Version="11.0.2"
* RabbitMQ.Client Version="5.1.0"
* Swashbuckle.AspNetCore Version="3.0.0"
* Swashbuckle.AspNetCore.Swagger Version="3.0.0"
* Microsoft.AspNetCore Version="2.1.2"
* Microsoft.EntityFrameworkCore.SqlServer.Design Version="1.1.6"
* Swashbuckle.AspNetCore.SwaggerUi Version="3.0.0"
* The second project, FinancialApi.UnitTests, was developed in .NET Core 2.1 with the following tool stack:

* Microsoft.EntityFrameworkCore Version="2.1.1"
* Microsoft.AspNetCore.Mvc.Testing Version="2.1.1"
* Microsoft.EntityFrameworkCore.Sqlite Version="2.1.1"
* Microsoft.EntityFrameworkCore.Sqlite.Core Version="2.1.1"
* Microsoft.VisualStudio.QualityTools.UnitTestFramework Version="11.0.50727.1"
* Moq Version="4.8.3"
* NUnit Version="3.10.1"
* NUnit.Console Version="3.8.0"
* NUnit3TestAdapter Version="3.10.0"
* Microsoft.NET.Test.Sdk Version="15.7.2"
* coverlet.msbuild Version="2.1.0"

## Pending

* The integrated test project was removed as it was not completed in time.
* Swagger configurations are incomplete, displaying some attributes incorrectly.
* Some JSON formatting has not been done.
* Some optimizations of dotnet in async processing have not been completed.
* Some Hangfire job controls have also not been implemented.

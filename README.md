# Aplicação de Lançamentos Financeiros

Essa API foi concebida seguindo (ou pelo menos tentando) seguir as especificações do [doc dentro do repository](docs/desafiofinanceiro.pdf). Aguns pontos que não ficaram claros apliquei a minha interpretação soobre o projeto.

## Levantando o Ambiente
A API foi dockerizada, por tanto o modo mais fácil de subir a aplicação é com docker e docker-compose. Uma vez com ambos instalados basta digitar o comando na raiz do projeto.

```sh
docker-compose up --build

```

## Consumindo a aplicação

A API expõem um conjuto de endpoint para serem consumindos do modo que achar mais conveniente. Eu sugiro que use a swaggerUI através da url<http://localhost:8080/swagger> que possui as especificação para consumir, juntamente com uma interface iterativa.

## Monitoração

Como a própria documnetação solicita, essa aplicação foi desenvolvida com o sistema de messageria rabbit mq, suas filas podem ser acompanhado pela em <http://localhost:15672> com as credenciais usermq/usermq.

Outra ferramenta utilizada foi o hangFire. Um library para processamento em background. Que foi utilizada para o consumir as menssagens do rabbiMQ.

## Tecnologia
Esse projeto é composto de uma solução visual studio c# com três projetos.

O projeto principal ** FinancialAPI ** foi desenvolvido em dotnet core 2.1 com a seguinte stack de ferramentas.

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

O segundo projeto, o ** FinanacialApi.UnitTests ** foi desenvolvido em dotnet core 2.1 com a seguinte stack de ferramentas.

* Microsoft.EntityFrameworkCore Version="2.1.1"
* Microsoft.AspNetCore.Mvc.Testing Version="2.1.1"
* Microsoft.EntityFrameworkCore.Sqlite Version="2.1.1"
* Microsoft.EntityFrameworkCore.Sqlite.Core Version="2.1.1"
* Microsoft.VisualStudio.QualityTools.UnitTestFramework Version="11.0.50727.1"
* Moq" Version="4.8.3"
* nunit" Version="3.10.1"
* NUnit.Console" Version="3.8.0"
* NUnit3TestAdapter" Version="3.10.0"
* Microsoft.NET.Test.Sdk" Version="15.7.2"
* coverlet.msbuild" Version="2.1.0"

## Pendências
* O projeto de teste integrado foi removido por não ter sido completado a tempo
* As confs do swagger está incompleto, exibindo alguns atributos indevidamente
* Algumas formatações do json não foram realizadas
* Algumas otimizações do dotnet no processamneto async não foram concluidas
* Alguns controles de jobs no Hangfire tambêm não foram realidadas

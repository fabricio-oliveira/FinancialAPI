# Aplicação de Lançamentos Financeiros

Essa API foi concebida seguindo (ou pelo menos tentando) seguir as especificações do [doc dentro do repository](../docs/desafiofinanceiro.pdf) 

## Levantando a aplicação
A API foi dockerizada, por tanto o modo mais fácil de subir a aplicação é com docker e docker-compose. Uma vez com ambos instalados basta digitar o comando na raiz do projeto.

```sh
docker-compose up --build

```


## Consumindo a aplicação

A API expõem um conjuto de endpoint para serem consumindos do modo que achar mais conveniente. Eu sugiro que use a swaggerUI através da url<http://localhost:8080/swaggerui> que possui as especificação para consumir, juntamente com uma interface iterativa.

## Monitoração

Como a própria documnetação solicita, essa aplicação foi desenvolvida com o sistema de messageria rabbit mq, e sua fila e consumo poder ser acompanhado pela url <http://localhost:15672>

Outra ferramenta utilizada foi o hangFire. Um gerenciador de jobs utilizado para processar as menssagens recuperadas do rabbiMQ.
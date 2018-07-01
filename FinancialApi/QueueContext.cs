using Microsoft.EntityFrameworkCore;
using System;
using FinancialApi.Model;
using RabbitMQ.Client;

namespace FinancialApi
{

    public class QueueContext
    {
        private ConnectionFactory _factory = null;
        public QueueContext(string StringConection)
        {
            var paramter = StringConection.Split(";");
            _factory = new ConnectionFactory()
            {
                HostName = paramter[0],
                UserName = paramter[1],
                Password = paramter[2]
            };
            using (var connection = _factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "payment",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

                channel.QueueDeclare(queue: "receipt",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
            }
        }
    }
}

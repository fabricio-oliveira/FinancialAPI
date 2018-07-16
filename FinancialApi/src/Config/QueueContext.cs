using RabbitMQ.Client;

namespace FinancialApi
{

    public class QueueContext
    {
        public IModel Channel { get; set; }

        public QueueContext(string StringConection)
        {
            var paramter = StringConection.Split(";");

            var factory = new ConnectionFactory()
            {
                HostName = paramter[0],
                UserName = paramter[1],
                Password = paramter[2]
            };

            PaymentQueueName = "payment";
            ReceiptQueueName = "receipt";
            InterestQueueName = "interst";

            ErrorQueueName = "error";


            var connection = factory.CreateConnection();
            Channel = connection.CreateModel();

            Channel.QueueDeclare(queue: PaymentQueueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            Channel.QueueDeclare(queue: ReceiptQueueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            Channel.QueueDeclare(queue: InterestQueueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            Channel.QueueDeclare(queue: ErrorQueueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
        }

        public string PaymentQueueName { get; }
        public string ReceiptQueueName { get; }
        public string InterestQueueName { get; }

        public string ErrorQueueName { get; }
    }
}

using RabbitMQ.Client;

namespace FinancialApi
{

    public class QueueContext
    {
        public IModel channel { get; set; }

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

            var connection = factory.CreateConnection();
            channel = connection.CreateModel();

            channel.QueueDeclare(queue: PaymentQueueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            channel.QueueDeclare(queue: ReceiptQueueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            //channel.QueueDeclare(queue: EntryQueueName,
            //durable: false,
            //exclusive: false,
            //autoDelete: false,
            //arguments: null);
            channel.QueueDeclare(queue: InterestQueueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
        }

        public string PaymentQueueName { get; }
        public string ReceiptQueueName { get; }
        public string EntryQueueName { get; }
        public string InterestQueueName { get; }
        public string ErrorQueueName { get; }
    }
}

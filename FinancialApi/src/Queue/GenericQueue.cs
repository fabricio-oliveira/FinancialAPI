using System.Text;
using RabbitMQ.Client;
using FinancialApi.Utils;
using RabbitMQ.Client.Events;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace FinancialApi.Queue
{


    public abstract class GenericQueue<T> where T : class
    {
        protected readonly QueueContext _context;
        AsyncEventingBasicConsumer _consumer;

        protected GenericQueue(QueueContext context)
        {
            _context = context;
        }

        protected abstract string QueueName();

        public void SetConsumer(AsyncEventHandler<BasicDeliverEventArgs> consumer)
        {
            _consumer = new AsyncEventingBasicConsumer(_context.Channel);
            _consumer.Received += consumer;
            _context.Channel.BasicConsume(queue: QueueName(),
                                               autoAck: true,
                                               consumer: _consumer);
        }

        public void Enqueue(T t, int? delay = null)
        {
            var body = Encoding.UTF8.GetBytes(t.ToJson());
            _context.Channel.BasicPublish(exchange: "",
                                          routingKey: QueueName(),
                                          basicProperties: Properties(delay),
                                          body: body);
        }

        public T Dequeue()
        {
            var data = _context.Channel.BasicGet(QueueName(), true);

            if (data == null)
                return null;

            var body = System.Text.Encoding.UTF8.GetString(data.Body);
            return JsonConvert.DeserializeObject<T>(body);

        }

        IBasicProperties Properties(int? val)
        {
            if (val == null) return null;

            var props = _context.Channel.CreateBasicProperties();
            var headers = new Dictionary<string, object>
            {
                { "x-delay", val }
            };
            props.Headers = headers;
            return props;
        }
    }
}

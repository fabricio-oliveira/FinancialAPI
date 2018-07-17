using System.Text;
using RabbitMQ.Client;
using FinancialApi.Utils;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace FinancialApi.Queue
{


    public abstract class GenericQueue<T> where T : class
    {
        protected readonly QueueContext _context;

        protected GenericQueue(QueueContext context)
        {
            _context = context;
        }

        protected abstract string QueueName();

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

using System.Text;
using RabbitMQ.Client;
using FinancialApi.Utils;
using RabbitMQ.Client.Events;
using System.Collections.Generic;
using System;

namespace FinancialApi.Queue
{


    public abstract class GenericQueue<T>
    {
        readonly QueueContext _context;
        readonly string _queueName;
        AsyncEventingBasicConsumer _consumer;

        protected GenericQueue(QueueContext context, string queueName)
        {
            this._context = context;
            this._queueName = queueName;
        }

        public void SetConsumer(AsyncEventHandler<BasicDeliverEventArgs> consumer)
        {
            this._consumer = new AsyncEventingBasicConsumer(_context.channel);
            this._consumer.Received += consumer;
            this._context.channel.BasicConsume(queue: _queueName,
                                               autoAck: true,
                                               consumer: this._consumer);
        }

        public void Enqueue(T t, int? delay = null)
        {
            var body = Encoding.UTF8.GetBytes(t.ToJson());
            _context.channel.BasicPublish(exchange: "",
                                          routingKey: _queueName,
                                          basicProperties: Properties(delay),
                                          body: body);
        }

        public T Dequeue()
        {
            var data = _context.channel.BasicGet(_queueName, true);

            var body = data != null ? System.Text.Encoding.UTF8.GetString(data.Body) : null;
            return Utils.StringUtil.FromJson<T>(body);

        }

        IBasicProperties Properties(int? val)
        {
            if (val == null) return null;

            var props = _context.channel.CreateBasicProperties();
            var headers = new Dictionary<string, object>
            {
                { "x-delay", val }
            };
            props.Headers = headers;
            return props;
        }
    }
}

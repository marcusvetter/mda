using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace Common
{
    public class Receiver
    {
        public delegate void OnReceiveMessage(byte[] message);

        protected IConnection Connection;
        protected IModel Model;
        protected string QueueName;

        protected bool IsReceiving;

        // used to pass messages back to UI for processing

        public Receiver(string hostName, string queueName)
        {
            QueueName = queueName;
            var connectionFactory = new ConnectionFactory();
            connectionFactory.HostName = hostName;
            Connection = connectionFactory.CreateConnection();
            Model = Connection.CreateModel();
            Model.QueueDeclare(QueueName, false, false, false, null);
        }

        public event OnReceiveMessage OnMessageReceived;

        //internal delegate to run the queue consumer on a seperate thread

        public void StartReceiving()
        {
            IsReceiving = true;
            var c = new ConsumeDelegate(Consume);
            c.BeginInvoke(null, null);
        }

        public void Consume()
        {
            var consumer = new QueueingBasicConsumer(Model);
            String consumerTag = Model.BasicConsume(QueueName, false, consumer);
            while (IsReceiving)
            {
                try
                {
                    var e = (BasicDeliverEventArgs) consumer.Queue.Dequeue();
                    IBasicProperties props = e.BasicProperties;
                    byte[] body = e.Body;
                    // ... process the message
                    OnMessageReceived(body);
                    Model.BasicAck(e.DeliveryTag, false);
                }
                catch (OperationInterruptedException ex)
                {
                    // The consumer was removed, either through
                    // channel or connection closure, or through the
                    // action of IModel.BasicCancel().
                    break;
                }
            }
        }

        public void Dispose()
        {
            IsReceiving = false;
            if (Connection != null)
                Connection.Close();
            if (Model != null)
                Model.Abort();
        }

        private delegate void ConsumeDelegate();
    }
}
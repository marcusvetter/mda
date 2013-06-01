using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace Common
{
    public class Sender : IDisposable
    {

        protected IModel Model;
        protected IConnection Connection;
        protected string QueueName;

        public Sender(string hostName, string queueName)
        {
            QueueName = queueName;
            var connectionFactory = new ConnectionFactory();
            connectionFactory.HostName = hostName;
            Connection = connectionFactory.CreateConnection();
            Model = Connection.CreateModel();
            Model.QueueDeclare(QueueName, false, false, false, null);
        }

        public void SendEvent(String evt)
        {
            var message = System.Text.Encoding.UTF8.GetBytes(evt);
            IBasicProperties basicProperties = Model.CreateBasicProperties();
            Model.BasicPublish("", QueueName, basicProperties, message);
        }
        public void Dispose()
        {
            if (Connection != null)
                Connection.Close();
            if (Model != null)
                Model.Abort();
        }
    }
}

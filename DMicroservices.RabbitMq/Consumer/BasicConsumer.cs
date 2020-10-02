using System;
using System.Text;
using DMicroservices.RabbitMq.Base;
using DMicroservices.Utils.Logger;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DMicroservices.RabbitMq.Consumer
{
    /// <summary>
    /// Consuming base
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BasicConsumer<T> : IBasicConsumer
    {
        public abstract string ListenQueueName { get; }

        public abstract bool AutoAck { get; }

        public abstract Action<T,ulong> DataReceivedAction { get; }


        /// <summary>
        /// Rabbitmq bağlantısı getirmek için oluşturulan class
        /// </summary>
        private RabbitMqConnection _rabbitMqConnection;

        /// <summary>
        /// Rabbitmq bağlantısı getirmek için oluşturulan class
        /// </summary>
        public RabbitMqConnection RabbitMqConnection
        {
            get
            {
                if (_rabbitMqConnection == null || !_rabbitMqConnection.IsConnected)
                {
                    _rabbitMqConnection = new RabbitMqConnection();
                }

                return _rabbitMqConnection;
            }
        }

        /// <summary>
        /// Modeli dinlemek için kullanıclan event
        /// </summary>
        private readonly EventingBasicConsumer _eventingBasicConsumer;

        private readonly IModel _rabitMqChannel;

        protected BasicConsumer()
        {
            try
            {
                if (string.IsNullOrEmpty(ListenQueueName))
                {
                    ElasticLogger.Instance.Info("Consumer QueueName was null");
                }
                _rabitMqChannel = RabbitMqConnection.GetChannel(ListenQueueName);
                _eventingBasicConsumer = new EventingBasicConsumer(_rabitMqChannel);
                _eventingBasicConsumer.Received += DocumentConsumerOnReceived;
                _rabitMqChannel.BasicConsume(ListenQueueName, AutoAck, _eventingBasicConsumer);
            }
            catch (Exception ex)
            {
                ElasticLogger.Instance.Error(ex, "RabbitMQ/RabbitmqConsumer");
            }

          
        }

        private void DocumentConsumerOnReceived(object sender, BasicDeliverEventArgs e)
        {
            var jsonData = Encoding.UTF8.GetString(e.Body.ToArray());
            DataReceivedAction(JsonConvert.DeserializeObject<T>(jsonData), e.DeliveryTag);
        }

        protected void BasicAck(ulong deliveryTag, bool multiple)
        {
            _rabitMqChannel.BasicAck(deliveryTag, multiple);
        }
    }
}

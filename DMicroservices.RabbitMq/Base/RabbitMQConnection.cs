﻿using System;
using DMicroservices.Utils.Logger;
using RabbitMQ.Client;

namespace DMicroservices.RabbitMq.Base
{
    /// <summary>
    /// Rabbitmq bağlantı backend classı
    /// </summary>
    public class RabbitMqConnection
    {
        #region Singleton Section
        private static readonly Lazy<RabbitMqConnection> _instance = new Lazy<RabbitMqConnection>(() => new RabbitMqConnection());


        public static RabbitMqConnection Instance => _instance.Value;
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public RabbitMqConnection()
        {
            GetConnection();
        }

        #endregion

        #region Properties

        private static readonly object _lockObj = new object();
        public IConnection Connection { get; set; }

        public bool IsConnected { get; set; } = false;

        #endregion

        #region Method

        /// <summary>
        /// Rabbitmq bağlantısı oluşturup döner
        /// </summary>
        /// <returns></returns>
        public IConnection GetConnection()
        {
            if (IsConnected)
                return Connection;
            try
            {
                lock (_lockObj)
                {
                    if (IsConnected)
                        return Connection;
                    ConnectionFactory connectionFactory = new ConnectionFactory
                    {
                        Uri = new Uri(Environment.GetEnvironmentVariable("RABBITMQ_URI"))
                    };
                    Connection = connectionFactory.CreateConnection();
                    IsConnected = true;
                    return Connection;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ElasticLogger.Instance.Error(ex, "RabbitmqConnection");
                return null;
            }
        }

        /// <summary>
        /// Channel oluşturup döner
        /// </summary>
        /// <returns></returns>
        public IModel GetChannel(string queueName)
        {
            IModel channel = Connection.CreateModel();
            channel.QueueDeclare(queueName, true, false, false, null);
            return channel;
        }

        #endregion
    }
}

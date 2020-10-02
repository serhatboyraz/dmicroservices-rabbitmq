namespace DMicroservices.RabbitMq.Consumer
{
    public interface IBasicConsumer
    {
        string ListenQueueName { get;}

        bool AutoAck { get; }

    }
}

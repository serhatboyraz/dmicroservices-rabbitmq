using System;
using DMicroservices.RabbitMq.Producer;

namespace DMicroservices.RabbitMq.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var exampleQueue = new ExampleConsumer();

            RabbitMqPublisher<ExampleModel>.Instance.Publish("ExampleQueue", new ExampleModel()
            {
                Message = "hello world"
            });

            Console.ReadLine();
        }
    }
}

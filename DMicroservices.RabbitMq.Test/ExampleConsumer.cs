using System;
using DMicroservices.RabbitMq.Consumer;

namespace DMicroservices.RabbitMq.Test
{
    class ExampleConsumer : BasicConsumer<ExampleModel>
    {
        public override string ListenQueueName => "ExampleQueue";

        public override bool AutoAck => true;

        public override Action<ExampleModel,ulong> DataReceivedAction => (model, deliveryTag) =>
        {
            Console.WriteLine(model.Message);

            //Send Ack.
            BasicAck(deliveryTag, false);
        };
    }
}

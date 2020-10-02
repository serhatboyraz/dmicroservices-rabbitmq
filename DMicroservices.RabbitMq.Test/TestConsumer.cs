using System;
using System.Collections.Generic;
using System.Text;
using DMicroservices.RabbitMq.Consumer;

namespace DMicroservices.RabbitMq.Test
{
    class TestConsumer : BasicConsumer<ExampleModel>
    {
        public override string ListenQueueName => "Test";
        public override bool AutoAck => false;
        public override Action<ExampleModel, ulong> DataReceivedAction => DataReceived;

        private void DataReceived(ExampleModel arg1, ulong arg2)
        {
            
        }
    }
}

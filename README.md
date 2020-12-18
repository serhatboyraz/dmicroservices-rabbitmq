
# dmicroservices-rabbitmq
RabbitMq Basic Conumser and Basic Publisher application base.

# a simple example
a. set environment variables
```java
   RABBITMQ_URI=amqp://root:root@127.0.0.1:18002
   **ELASTIC_URI=http://elasticsearch:9200
   **LOG_INDEX_FORMAT=rmq-{0:yyyy.MM.dd}
```
** if you want use elastic serilogger.

b. declare a model
```java
    class ExampleModel
    {
        public string Message { get; set; }
    }
```

c. declare a consumer class(must be implemented from BasicConsumer<T>)
```java
    class ExampleConsumer : BasicConsumer<ExampleModel>
    {
        public override string ListenQueueName => "ExampleQueue";
        public override bool AutoAck => true;
        public override Action<ExampleModel,ulong> DataReceivedAction => DataReceived;
        private void DataReceived(ExampleModel model, ulong deliveryTag)
        {
            Console.WriteLine(model.Message);
        }
    }
```
d. register this consumer from registry.
```java
ConsumerRegistry.Instance.Register(typeof(ExampleModel));
```
e. push any message to queue.
```java
RabbitMqPublisher<ExampleModel>.Instance.Publish("Test",new ExampleModel()
            {
                Message = "hello world."
            });
```

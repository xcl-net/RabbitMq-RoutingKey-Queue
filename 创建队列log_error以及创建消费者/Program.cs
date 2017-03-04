using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace 创建队列log_error以及创建消费者
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("创建队列log_error以及创建消费者...");
            var factory = new ConnectionFactory
            {
                HostName = "127.0.0.1",
                UserName = "guest",
                Password = "guest"
            };

            //创建连接
            var connection = factory.CreateConnection();
            //创建channel
            var channel = connection.CreateModel();
            //声明交换机 因为RabbitMq 已经有了自定义的amqp  default exchange
            channel.ExchangeDeclare("myexchange", ExchangeType.Direct, true, false, null);

            //在channel上创建一个队列
            channel.QueueDeclare("log_error", true, false, false, null);

            //将队列和交换机绑定，通过routingKey
            channel.QueueBind("log_error", "myexchange", "error", null);

            //队列的消费者
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (sender, e) =>
            {
                var msg = Encoding.UTF8.GetString(e.Body);
                Console.WriteLine(msg);
            };

            channel.BasicConsume("log_error", true, consumer);
            Console.Read();

        }
    }
}

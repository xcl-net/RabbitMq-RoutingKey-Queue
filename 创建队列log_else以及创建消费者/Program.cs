using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace 创建队列log_else以及创建消费者
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("创建队列log_else以及创建消费者...");
            var factory = new ConnectionFactory
            {
                HostName = "127.0.0.1",
                UserName = "guest",
                Password = "guest"
            };
            //创建连接
            var connection = factory.CreateConnection();
            //创建一个channel
            var channel = connection.CreateModel();
            //第三步：声明一个交换机 ，不声明，RabbitMq有自定义的amqp default exchange
            channel.ExchangeDeclare("myexchange",ExchangeType.Direct,true,false,null);

            //第四步：创建一个队列（queue）
            channel.QueueDeclare("log_else", true, false, false, null);

            //初始化，路由key  routingKey
            var arr=new string[3] {"debug","info","warning"};

            //将初始化的路由key绑定到“log_else”队列中
            for (int i = 0; i < arr.Length; i++)
            {
                channel.QueueBind("log_else","myexchange",arr[i]);
            }

            //指定一个消费者
            var consumer = new EventingBasicConsumer(channel);

            //消费者，事件
            consumer.Received += (sender, e) =>
            {
                var msg = Encoding.UTF8.GetString(e.Body);
                Console.WriteLine(msg);
            };

            channel.BasicConsume("log_else", true, consumer);

            Console.Read();


        }


    }
}

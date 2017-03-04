using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace PublishMsg
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory
            {
                HostName = "127.0.0.1",
                UserName = "guest",
                Password = "guest"
            };
            //创建connetion
            var connection = factory.CreateConnection();
            //创建一个channel
            var channel = connection.CreateModel();
            for (int i = 0; i < 100; i++)
            {
                var msg = Encoding.UTF8.GetBytes(string.Format("{0}:{1}", i, "消息体"));

                var level = i % 13 == 0 ? "error" : "info"; //方便演示
                Thread.Sleep(1000);
                //根绝不同的routingKey发送消息
                channel.BasicPublish("myexchange", routingKey: level, basicProperties: null, body: msg);
                Console.WriteLine(i);
            }
            connection.Dispose();
            channel.Dispose();
        }
    }
}

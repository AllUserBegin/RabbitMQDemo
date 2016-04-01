using ClassLibrary;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QueueReceive
{
    class Program
    {
        static void Main(string[] args)
        {
            //try
            //{
                ConnectionFactory factory = new ConnectionFactory();
                factory.HostName = Constants.MqHost;
                //factory.HostName = Constants.MqHost;
                //factory.Port = Constants.MqPort;
                //factory.UserName = Constants.MqUserName;
                //factory.Password = Constants.MqPwd;
                using (IConnection conn = factory.CreateConnection())
                {
                    using (IModel channel = conn.CreateModel())
                    {
                        //在MQ上定义一个持久化队列，如果名称相同不会重复创建
                        //durable 消息持久化
                        channel.QueueDeclare("MyFirstQueue", true, false, false, null);

                        //输入1，那如果接收一个消息，但是没有应答，则客户端不会收到下一个消息
                        channel.BasicQos(0, 1, false);

                        Console.WriteLine("Listening...");

                       
                        //在队列上定义一个消费者
                        QueueingBasicConsumer consumer = new QueueingBasicConsumer(channel);
                        //消费队列，并设置应答模式为程序主动应答
                        channel.BasicConsume("MyFirstQueue", false, consumer);
                      

                        while (true)
                        {
                            //阻塞函数，获取队列中的消息
                            BasicDeliverEventArgs ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();
                            byte[] bytes = ea.Body;
                            string str = Encoding.UTF8.GetString(bytes);
                            RequestMsg msg = JsonConvert.DeserializeObject<RequestMsg>(str);
                            Console.WriteLine("HandleMsg:" + str);
                            //回复确认
                            channel.BasicAck(ea.DeliveryTag, false);
                        }
                    }
                }
            //}
            //catch (Exception e1)
            //{
            //    Console.WriteLine(e1.ToString());
            //}
            Console.ReadLine();
        }
    }
}

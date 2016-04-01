using ClassLibrary;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QueueClient
{
    class Program
    {
        public static void Run1() 
        { 
            Thread.Sleep(100000); 
        }

        public static void Run2() 
        { 
            Thread.Sleep(100000);
        }

        public static void Run3()
        {
            Thread.Sleep(100000);
        }

        static void Main(string[] args)
        {
            //var task = Task.Factory.StartNew(() =>
            //{
            //    var task1 = Task.Factory.StartNew(Run1);
            //    var task2 = Task.Factory.StartNew(Run2);
            //    var task3 = Task.Factory.StartNew(Run3);

                

            //    Task.WaitAll(new Task[] { task1, task2, task3 });
            //});

            //TaskFactory taskFactory = new TaskFactory();
            //Task[] tasks = new Task[]
            //{                
            //    taskFactory.StartNew(() => Run1()),    
            //    taskFactory.StartNew(() => Run2()),
            //    taskFactory.StartNew(() => Run3()) 
            //};
            //taskFactory.ContinueWhenAll(tasks,null);

            //Console.Read();


            //try
            //{
                ConnectionFactory factory = new ConnectionFactory();
                factory.HostName = Constants.MqHost;
                factory.Port = Constants.MqPort;
                factory.UserName = Constants.MqUserName;
                factory.Password = Constants.MqPwd;
                //创建一个连接
                using (IConnection conn = factory.CreateConnection())
                {
                    //创建并返回一个新的通道、会话和模型。
                    using (IModel channel = conn.CreateModel())
                    {
                        
                        
                        //在MQ上定义一个持久化队列，如果名称相同不会重复创建
                        //durable 消息持久化
                        channel.QueueDeclare("MyFirstQueue", true, false, false, null);
                        int k = 0;
                        while (k<10000)
                        {
                            string customStr = k.ToString();// Console.ReadLine();
                            RequestMsg requestMsg = new RequestMsg();
                            requestMsg.Name = string.Format("Name_{0}", customStr);
                            requestMsg.Code = string.Format("Code_{0}", customStr);
                            string jsonStr = JsonConvert.SerializeObject(requestMsg);
                            byte[] bytes = Encoding.UTF8.GetBytes(jsonStr);

                            //设置消息持久化
                            IBasicProperties properties = channel.CreateBasicProperties();
                            ////数据持久化  (DeliveryMode:非持续性：1 持续性:2)  
                            properties.DeliveryMode = 2;

                            //exchange：交换机名字,exchangeType类型, durable是否持久化,autoDelete不使用时是否自动删除
                           // channel.ExchangeDeclare("", "topic",true);
                            //exchange：交换机名字 routeKey：路由关键字 properties 设置消息持续性  bytes :消息主体  
                            channel.BasicPublish("", "MyFirstQueue", properties, bytes);

                            //channel.BasicPublish("", "MyFirstQueue", null, bytes);

                            Console.WriteLine("消息已发送：" + requestMsg.ToString());
                            k++;
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

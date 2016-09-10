﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Thrift.Client;
using Thrift.Protocol;
using Thrift.Transport;
using System.Net;

namespace Thrift.ClientWin
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    using (var svc = ThriftClientManager<ThriftTest.GameThriftService.Client>.GetClient("GameThriftService"))
                    {
                        svc.Client.Get(1);
                     //   Console.WriteLine("Get:" + Newtonsoft.Json.JsonConvert.SerializeObject(svc.Client.Get(1)));
                        //Console.WriteLine("GetALL:" + Newtonsoft.Json.JsonConvert.SerializeObject(svc.Client.GetALL()));
                        Console.WriteLine("true");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("false"+ex.Message);
                }
                System.Threading.Thread.Sleep(5000);
            }

            int het = 500;

            //var stopwatch = new Stopwatch();
            //stopwatch.Start();

            //var countdown = new CountdownEvent(het);
            //ThreadPool.SetMinThreads(1000, 1000);
            //ThreadPool.SetMaxThreads(1000, 1000);

            //for (int i = 0; i < het; i++)
            //{

            //    ThreadPool.QueueUserWorkItem((obj) =>
            //    {
            //        using (var svc = ThriftClientManager<ThriftTest.GameThriftService.Client>.GetClient("GameThriftService"))
            //        {
            //      svc.Client.Get(1);
            //        svc.Client.GetALL();
            //        }
            //        countdown.Signal();
            //    });

            //}


            //while (!countdown.IsSet) ;

            //stopwatch.Stop();

            //Console.WriteLine(stopwatch.ElapsedMilliseconds);
            //Console.WriteLine("over");



            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var threads = new List<Thread>();
            var countdown = new CountdownEvent(het);
            for (int i = 0; i < het; i++)
            {
                threads.Add(new Thread(() =>
                {
                    using (var svc = ThriftClientManager<ThriftTest.GameThriftService.Client>.GetClient("GameThriftService"))
                    {
                        svc.Client.Get(1);
                        svc.Client.GetALL();
                    }
                    countdown.Signal();
                }));
            }

            for (int i = 0; i < het; i++)
            {
                threads[i].Start();
            }

            while (!countdown.IsSet) ;
            stopwatch.Stop();

            Console.WriteLine(stopwatch.ElapsedMilliseconds);
            Console.WriteLine("over");


            Console.ReadLine();
        }
    }
}

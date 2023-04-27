using System;
using System.Collections.Generic;
using System.Threading;

namespace ProducerConsumer
{
    class Program
    {
        private static Queue<int> queue = new Queue<int>();
        private static object queueLock = new object();
        private static Random random = new Random();

        static void Main(string[] args)
        {
            Thread producerThread = new Thread(Producer);
            Thread consumerThread = new Thread(Consumer);

            producerThread.Start();
            consumerThread.Start();

            producerThread.Join();
            consumerThread.Join();
        }

        private static void Producer()
        {
            while (true)
            {
                int randomNumber = random.Next(1, 101);
                lock (queueLock)
                {
                    queue.Enqueue(randomNumber);
                    Console.WriteLine($"Producer: {randomNumber}");
                }
                Thread.Sleep(1000);
            }
        }

        private static void Consumer()
        {
            while (true)
            {
                int number;
                lock (queueLock)
                {
                    if (queue.Count > 0)
                    {
                        number = queue.Dequeue();
                        Console.WriteLine($"Consumer: {number}");
                    }
                }
                Thread.Sleep(500);
            }
        }
    }
}


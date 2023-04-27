using System;
using System.Threading;

namespace TrafficIntersection
{
    class Program
    {
        private static Random random = new Random();
        private static object trafficLightLock = new object();
        private static SemaphoreSlim semaphore = new SemaphoreSlim(2); 

        static void Main(string[] args)
        {
            Thread[] trafficLightThreads = new Thread[4];

            for (int i = 0; i < 4; i++)
            {
                int trafficLightId = i + 1;
                trafficLightThreads[i] = new Thread(() => TrafficLightController(trafficLightId));
                trafficLightThreads[i].Start();
            }

            for (int i = 0; i < 4; i++)
            {
                trafficLightThreads[i].Join();
            }
        }

        private static void TrafficLightController(int trafficLightId)
        {
            while (true)
            {
                Thread.Sleep(random.Next(1000, 5000)); 

                lock (trafficLightLock)
                {
                    Console.WriteLine($"Світлофор {trafficLightId}: Червоний");

                    Thread.Sleep(1000); 

                    Console.WriteLine($"Світлофор {trafficLightId}: Зелений");

                    for (int i = 0; i < 3; i++) 
                    {
                        semaphore.Wait();

                        Console.WriteLine($"Автомобіль {i + 1} проїхав через світлофор {trafficLightId}");

                        semaphore.Release();
                    }
                }
            }
        }
    }
}

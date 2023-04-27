using System;
using System.Threading;

namespace MultithreadedQuickSort
{
    class Program
    {
        private static object arrayLock = new object();

        static void Main(string[] args)
        {
            int[] array = { 7, 10, 1, 0, 13, 2, 16, 5, 8 };
            Console.WriteLine("Вихідний масив: " + string.Join(", ", array));

            ParallelQuickSort(array, 0, array.Length - 1);

            Console.WriteLine("Відсортований масив: " + string.Join(", ", array));
        }

        private static void ParallelQuickSort(int[] array, int low, int high)
        {
            if (low < high)
            {
                int pivotIndex = Partition(array, low, high);

                Thread leftThread = new Thread(() => ParallelQuickSort(array, low, pivotIndex - 1));
                Thread rightThread = new Thread(() => ParallelQuickSort(array, pivotIndex + 1, high));

                leftThread.Start();
                rightThread.Start();

                leftThread.Join();
                rightThread.Join();
            }
        }

        private static int Partition(int[] array, int low, int high)
        {
            int pivot = array[high];
            int i = low - 1;

            for (int j = low; j <= high - 1; j++)
            {
                if (array[j] < pivot)
                {
                    i++;
                    lock (arrayLock)
                    {
                        Swap(array, i, j);
                    }
                }
            }

            lock (arrayLock)
            {
                Swap(array, i + 1, high);
            }

            return i + 1;
        }

        private static void Swap(int[] array, int a, int b)
        {
            int temp = array[a];
            array[a] = array[b];
            array[b] = temp;
        }
    }
}

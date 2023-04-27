using System;
using System.Threading;

namespace ParallelMatrixMultiplication
{
    class Program
    {
        private static SemaphoreSlim semaphore;
        private static object monitorLock = new object();

        static void Main(string[] args)
        {
            int[,] matrixA = {
                { 10, 17, 9 },
                { 3, 6, 22 },
                { 28, 2, 13 }
            };

            int[,] matrixB = {
                { 4, 81, 0 },
                { 31, 13, 5 },
                { 9, 1, 2 }
            };

            int rowCount = matrixA.GetLength(0);
            int colCount = matrixB.GetLength(1);
            int[,] result = new int[rowCount, colCount];

            semaphore = new SemaphoreSlim(Environment.ProcessorCount);

            int cellCount = rowCount * colCount;
            Thread[] threads = new Thread[cellCount];

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < colCount; j++)
                {
                    int row = i;
                    int col = j;

                    semaphore.Wait();

                    threads[i * colCount + j] = new Thread(() =>
                    {
                        MultiplyCell(matrixA, matrixB, result, row, col);
                        semaphore.Release();
                    });

                    threads[i * colCount + j].Start();
                }
            }

            for (int i = 0; i < threads.Length; i++)
            {
                threads[i].Join();
            }

            Console.WriteLine("Результат множення матриць:");
            PrintMatrix(result);
        }

        private static void MultiplyCell(int[,] matrixA, int[,] matrixB, int[,] result, int row, int col)
        {
            int sum = 0;
            int length = matrixA.GetLength(1);

            for (int i = 0; i < length; i++)
            {
                sum += matrixA[row, i] * matrixB[i, col];
            }

            lock (monitorLock)
            {
                result[row, col] = sum;
            }
        }

        private static void PrintMatrix(int[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write($"{matrix[i, j]} ");
                }
                Console.WriteLine();
            }
        }
    }
}

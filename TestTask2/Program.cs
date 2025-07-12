using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask2
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public static class Server
    {
        private static int count = 0;

        // ReaderWriterLockSlim позволяет реализовать параллельное чтение и эксклюзивную запись
        private static ReaderWriterLockSlim rwLock = new ReaderWriterLockSlim();

        // Метод для безопасного чтения переменной count
        public static int GetCount()
        {
            rwLock.EnterReadLock();
            try
            {
                // Чтение count
                return count;
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        // Метод для безопасного добавления значения к count
        public static void AddToCount(int value)
        {
            rwLock.EnterWriteLock();
            try
            {
                // Запись в count (добавление)
                count += value;
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }
    }

    class Program
    {
        static void Main()
        {
            // Запуск нескольких читателей и писателей параллельно
            Task[] tasks = new Task[]
            {
            Task.Run(() => Reader("Reader1")),
            Task.Run(() => Reader("Reader2")),
            Task.Run(() => Writer("Writer1")),
            Task.Run(() => Writer("Writer2"))
            };

            Task.WaitAll(tasks);

            Console.WriteLine($"\nИтоговое значение count: {Server.GetCount()}");
        }

        // Метод, имитирующий чтение count
        static void Reader(string name)
        {
            for (int i = 0; i < 5; i++)
            {
                int value = Server.GetCount();
                Console.WriteLine($"{name} прочитал: {value}");
                Thread.Sleep(100); // Пауза для имитации работы
            }
        }

        // Метод, имитирующий добавление к count
        static void Writer(string name)
        {
            for (int i = 0; i < 5; i++)
            {
                Server.AddToCount(1);
                Console.WriteLine($"{name} увеличил count на 1");
                Thread.Sleep(150); // Пауза для имитации работы
            }
        }
    }

}

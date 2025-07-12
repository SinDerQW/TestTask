using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите строку: ");
            string str = Console.ReadLine();

            Console.WriteLine($"Компрессия: {Compress(str)}");
        }

        // Метод компрессии
        public static string Compress(string input)
        {
            if (string.IsNullOrEmpty(input)) return "";

            StringBuilder result = new StringBuilder();
            char currentChar = input[0];
            int count = 1;

            for (int i = 1; i < input.Length; i++)
            {
                if (input[i] == currentChar)
                {
                    count++;
                }
                else
                {
                    result.Append(currentChar);
                    if (count > 1)
                        result.Append(count);
                    currentChar = input[i];
                    count = 1;
                }
            }

            // Добавим последнюю группу
            result.Append(currentChar);
            if (count > 1)
                result.Append(count);

            return result.ToString();
        }
    }
}

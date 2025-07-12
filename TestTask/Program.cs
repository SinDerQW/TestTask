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
            Console.WriteLine($"Декомпрессия: {Decompress(Compress(str))}");
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

        public static string Decompress(string input)
        {
            if (string.IsNullOrEmpty(input)) return "";

            StringBuilder result = new StringBuilder();
            char currentChar = '\0';
            int count = 0;

            for (int i = 0; i < input.Length; i++)
            {
                if (char.IsLetter(input[i]))
                {
                    // Если до этого был символ и число, добавляем его в результат
                    if (currentChar != '\0')
                    {
                        result.Append(new string(currentChar, count == 0 ? 1 : count));
                    }

                    currentChar = input[i];
                    count = 0; // Сбросить счётчик
                }
                else if (char.IsDigit(input[i]))
                {
                    // Собираем цифры в число (на случай count > 9)
                    int digit = input[i] - '0';
                    count = count * 10 + digit;
                }
            }

            // Добавить последнюю группу
            if (currentChar != '\0')
            {
                result.Append(new string(currentChar, count == 0 ? 1 : count));
            }

            return result.ToString();
        }

    }
}

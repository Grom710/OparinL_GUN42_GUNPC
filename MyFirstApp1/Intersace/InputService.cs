// CasinoApp/Services/InputService.cs
using System;

namespace CasinoApp.Services
{
    public static class InputService
    {
        public static string ReadString(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine().Trim();
        }

        public static decimal ReadDecimal(string prompt)
        {
            decimal value;
            while (true)
            {
                Console.Write(prompt);
                if (decimal.TryParse(Console.ReadLine(), out value) && value > 0)
                    return value;
                Console.WriteLine("Некорректный ввод. Введите положительное число.");
            }
        }

        public static int ReadInt(string prompt, int min, int max)
        {
            int value;
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out value) && value >= min && value <= max)
                    return value;
                Console.WriteLine($"Введите число от {min} до {max}.");
            }
        }
    }
}
//See https://aka.ms/new-console-template for more information
//using System;
//using System.Net.NetworkInformation;

//class PROGRAM
//{
//    static void Main()
//    {
//        int n = 10;
//        int a = 0, b = 1;
//        int count = 0;
//        Console.WriteLine("First numbers to Fb");
//        while (count < n)
//        {
//            Console.WriteLine(a + " ");
//            int next = a + b;
//            a = b;
//            b = next;
//            count++;
//        }


//    }
//}

//for (int i = 2; i <= 20; i += 2)
//{
//    Console.WriteLine(i);
//}

//for (int i = 1; i <= 5; i++)
//{
//    for (int j = 1; j <= 5; j++)
//    {
//        Console.Write($"{i} * {j} = {i * j}\t");
//    }
//    Console.WriteLine();
//}

//using System;

//class Program
//{
//    static void Main()
//    {
//        string password = "qwerty";
//        string userInput;

//        do
//        {
//            Console.Write("Введите пароль: ");
//            userInput = Console.ReadLine();

//            if (userInput != password)
//            {
//                Console.WriteLine("Неверный пароль. Попробуйте ещё раз.");
//            }
//        }
//        while (userInput != password);

//        Console.WriteLine("Пароль принят. Доступ разрешён!");
//    }
//}
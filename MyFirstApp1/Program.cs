using System.Runtime.ExceptionServices;

namespace HomeWork
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Здесь массивы заданий 1-4
            //int[] Fib = new int[8];
            //Fib[0] = 0;
            //Fib[1] = 1;
            //for (int i = 2; i < 8; i++) 
            //{
            //Fib [i] = Fib[i - 1] + Fib[i - 2 ];
            //}
            //Console.WriteLine("First numbers Fib");
            //foreach (var num in Fib)
            //{
            //    Console.Write(num + "");
            //}

            string[] months = new string[]
            { "January", "February","March","April","May","June","July","August","September","October","November","December"
            };
            foreach(string month in months)
            {
                Console.WriteLine(month);
            }









        }
    }
}
        //    массивы для заданий 5 и 6.
        //    int[] array = { 1, 2, 3, 4, 5 };
        //    int[] array2 = { 7, 8, 9, 10, 11, 12, 13 };
        //    var result = CopyArrays(array, array2, 2);
        //    Выведите результат

        //    string[] sample = { "", "" };
        //    ResizeArray(ref array, /* подставьте число вторым аргументов  */ );
        //    Что же будет выведено?
        //}
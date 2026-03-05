// See https://aka.ms/new-console-template for more information
using System;
using System.Net.NetworkInformation;

int[] array1 = new int[] { 32, 111, 25, 17 };


for (int index = array1.Length - 1; index >= 0; index--)
    if (array1[index] == 111)
{
    continue
}
Console.WriteLine(array1[index]);



//int value = 150;
//int result = 0;
//int index = 0;
//do 
//{
//result += array1[index];
//} while (result < value);
//Console.WriteLine(result.ToString());

//while (index < array1.Length   ) 
//{
//    if (array1[index] >= 111)
//    {
//        index++;
//        continue;
//    }
//    Console.WriteLine( array1[index] );
//    index++;

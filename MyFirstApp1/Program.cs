using System;
using System.Collections.Generic;

namespace HomeWork
{
    internal class Program
    {
        private class ListTask
        {
            public void TaskLoop()
            {
                List<string> items = new List<string> { "Яблоко", "Банан", "Вишня" };

                Console.WriteLine("Задание 1: Работа со списком");
                Console.WriteLine("Текущий список: " + string.Join(", ", items));
                Console.WriteLine("Введите строку, чтобы добавить ее в конец списка. Введите '-exit' для выхода.");

                while (true)
                {
                    string input = Console.ReadLine() ?? "";

                    if (input.ToLower() == "-exit")
                    {
                        break;
                    }

                    items.Add(input);
                    Console.WriteLine("Список обновлен: " + string.Join(", ", items));

                    Console.WriteLine("Теперь введите еще одну строку, чтобы добавить ее в середину списка.");
                    string middleInput = Console.ReadLine() ?? "";

                    if (middleInput.ToLower() == "-exit")
                    {
                        break;
                    }

                    int middleIndex = items.Count / 2;
                    items.Insert(middleIndex, middleInput);

                    Console.WriteLine("Строка добавлена в середину. Итоговый список: " + string.Join(", ", items));
                    Console.WriteLine("Введите '-exit' для выхода или любую строку, чтобы повторить сначала.");
                }
                Console.WriteLine("Выход из Задания 1.");
            }
        }

        private class DictionaryTask
        {
            public void TaskLoop()
            {
                Dictionary<string, int> students = new Dictionary<string, int>();
                Console.WriteLine("Задание 2: Словарь оценок");
                Console.WriteLine("Введите имя студента и его оценку (от 2 до 5). Формат: Имя Оценка. Введите '-exit' для выхода.");

                while (true)
                {
                    string input = Console.ReadLine() ?? "";

                    if (input.ToLower() == "-exit")
                    {
                        break;
                    }

                    string[] parts = input.Split();

                    if (parts.Length != 2)
                    {
                        Console.WriteLine("Неверный формат. Используйте: Имя Оценка");
                        continue;
                    }

                    string name = parts[0];

                    if (!int.TryParse(parts[1], out int grade) || grade < 2 || grade > 5)
                    {
                        Console.WriteLine("Ошибка: Оценка должна быть целым числом от 2 до 5.");
                        continue;
                    }

                    students[name] = grade;
                    Console.WriteLine($"Оценка {grade} для студента {name} сохранена.");

                    Console.WriteLine("Введите имя студента, чтобы узнать его оценку:");
                    string searchName = Console.ReadLine() ?? "";

                    if (searchName.ToLower() == "-exit")
                    {
                        break;
                    }

                    if (students.TryGetValue(searchName, out int foundGrade))
                    {
                        Console.WriteLine($"Оценка студента {searchName}: {foundGrade}");
                    }
                    else
                    {
                        Console.WriteLine($"Студент с именем {searchName} не найден.");
                    }

                    Console.WriteLine("Введите '-exit' для выхода или новые данные в формате 'Имя Оценка'.");
                }
                Console.WriteLine("Выход из Задания 2.");
            }
        }

        private class LinkedListTask
        {
            private class Node
            {
                public string Data { get; set; }
                public Node? Next { get; set; }
                public Node? Previous { get; set; }

                public Node(string data)
                {
                    Data = data;
                }
            }

            private Node? head; 
            private Node? tail; 

            public void TaskLoop()
            {
                Console.WriteLine("Задание 3: Двусвязный список");

                List<string> elements = new List<string>();

                Console.WriteLine("Пожалуйста, введите от 3 до 6 элементов списка (по одному в строке):");

                while (elements.Count < 3)
                {
                    string input = Console.ReadLine() ?? "";
                    if (input.ToLower() == "-exit") return;
                    elements.Add(input);
                }

                Console.WriteLine("Список из 3 элементов создан. Введите еще до 3 элементов или '-exit':");
                while (elements.Count < 6)
                {
                    string input = Console.ReadLine() ?? "";
                    if (input.ToLower() == "-exit") break;
                    elements.Add(input);
                }

                foreach (var item in elements)
                {
                    AddToEnd(item);
                }

                Console.WriteLine("\n--- Прямой порядок ---");
                Node? current = head;
                while (current != null)
                {
                    Console.Write(current.Data + " ");
                    current = current.Next;
                }

                Console.WriteLine("\n\n--- Обратный порядок ---");
                current = tail;
                while (current != null)
                {
                    Console.Write(current.Data + " ");
                    current = current.Previous;
                }

                Console.WriteLine("\n\nЗадание выполнено. Нажмите Enter для выхода или введите '-exit'.");
                string finalInput = Console.ReadLine() ?? "";
                if (finalInput.ToLower() == "-exit")
                {
                    return;
                }
            }

            private void AddToEnd(string data)
            {
                Node newNode = new Node(data);

                if (head == null)
                {
                    head = tail = newNode;
                }
                else
                {

                    (tail!).Next = newNode;
                    newNode.Previous = tail;
                    tail = newNode;
                }
            }
        }


        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Выберите задание:");
                Console.WriteLine("1 - Работа со списком строк");
                Console.WriteLine("2 - Словарь оценок студентов");
                Console.WriteLine("3 - Двусвязный список");
                Console.WriteLine("0 - Выход из программы");
                Console.Write("\nВаш выбор: ");

                string choice = Console.ReadLine() ?? "";

                switch (choice)
                {
                    case "1":
                        CheckTaskFirst();
                        break;
                    case "2":
                        CheckTaskSecond();
                        break;
                    case "3":
                        CheckTaskThird();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор. Нажмите Enter для продолжения.");
                        Console.ReadLine();
                        break;
                }
            }
        }


        private static void CheckTaskFirst()
        {
            var task = new ListTask();
            task.TaskLoop();
            Console.ReadLine(); 
        }

        private static void CheckTaskSecond()
        {
            var task = new DictionaryTask();
            task.TaskLoop();
            Console.ReadLine(); 
        }

        private static void CheckTaskThird()
        {
            var task = new LinkedListTask();
            task.TaskLoop();
            Console.ReadLine(); 
        }
    }
}

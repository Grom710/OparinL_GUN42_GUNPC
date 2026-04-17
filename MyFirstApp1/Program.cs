using System;
using System.Text;

// Задание 1
static string ConcatenateStrings(string first, string second)
{
    return first + second;
}

// Задание 2
static string GreetUser(string name, int age)
{
    return $"Hello, {name}!\nYou are {age} years old.";
}

// Задание 3
static string GetStringInfo(string input)
{
    int length = input.Length;
    string upper = input.ToUpper();
    string lower = input.ToLower();

    return $"Length: {length}, Uppercase: {upper}, Lowercase: {lower}";
}

// Задание 4
static string GetFirstFiveChars(string input)
{
    if (string.IsNullOrEmpty(input) || input.Length < 5)
        return input;

    return input.Substring(0, 5);
}

// Задание 5
static StringBuilder JoinArrayToSentence(string[] words)
{
    StringBuilder sentenceBuilder = new StringBuilder();

    foreach (string word in words)
    {
        sentenceBuilder.Append(word).Append(" ");
    }

    if (sentenceBuilder.Length > 0)
    {
        sentenceBuilder.Length--; // Убираем лишний пробел в конце
    }

    return sentenceBuilder;
}

// Задание 6
static string ReplaceWords(string inputString, string wordToReplace, string replacementWord)
{
    return inputString.Replace(wordToReplace, replacementWord);
}


// Точка входа в программу (Операторы верхнего уровня)
Console.WriteLine("=== Проверка всех заданий ===\n");
CheckAllTasks();
Console.WriteLine("\nНажмите Enter, чтобы выйти...");
Console.ReadLine();


// Вспомогательный метод для запуска всех примеров
static void CheckAllTasks()
{
    // --- Проверка Задания 1 ---
    string result1 = ConcatenateStrings("Hello", "World");
    Console.WriteLine("Задание 1 (Конкатенация):");
    Console.WriteLine(result1); // Ожидаем: HelloWorld
    Console.WriteLine("---");

    // --- Проверка Задания 2 ---
    string result2 = GreetUser("Иван", 25);
    Console.WriteLine("Задание 2 (Приветствие):");
    Console.WriteLine(result2);
    Console.WriteLine("---");

    // --- Проверка Задания 3 ---
    string result3 = GetStringInfo("Программирование");
    Console.WriteLine("Задание 3 (Информация о строке):");
    Console.WriteLine(result3);
    Console.WriteLine("---");

    // --- Проверка Задания 4 ---
    string result4 = GetFirstFiveChars("VisualStudio");
    Console.WriteLine("Задание 4 (Первые 5 символов):");
    Console.WriteLine(result4); // Ожидаем: Visual
    Console.WriteLine("---");

    // --- Проверка Задания 5 ---
    string[] wordsArray = { "C#", "is", "awesome" };
    StringBuilder result5 = JoinArrayToSentence(wordsArray);
    Console.WriteLine("Задание 5 (StringBuilder):");
    Console.WriteLine(result5.ToString()); // Ожидаем: C# is awesome
    Console.WriteLine("---");

    // --- Проверка Задания 6 ---
    string result6 = ReplaceWords("Hello world world", "world", "universe");
    Console.WriteLine("Задание 6 (Замена слов):");
    Console.WriteLine(result6); // Ожидаем: Hello universe universe
}
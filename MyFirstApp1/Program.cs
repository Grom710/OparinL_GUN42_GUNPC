// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
Console.WriteLine("Netologia 1000");
{
    if (!Int32.TryParse(Console.ReadLine(), out var a))
    {
        Console.WriteLine("Not a number!");
        return;
    }

    if (!Int32.TryParse(Console.ReadLine(), out var b))
    {
        Console.WriteLine("Not a number");
        return;
    }

    var s = (Console.ReadLine());
    var boolVar = true;
    if (s.Length == 0 || s.Length > 1 && !boolVar)
    {
        Console.WriteLine("Wrong sign");
        return;
    }

    switch (s[0])
    {
        case '+':
            Console.WriteLine("Result of {0} + {1} = {2}", a, b, a + b);
            break;
        case '-':
            Console.WriteLine("Result of {0} + {1} = {2}", a, b, a - b);
            break;
        case '*':
            Console.WriteLine("Result of {0} * {1} = {2}", a, b, a * b);
            break;
        case '/':
            Console.WriteLine("Result of {0} / {1} = {2}", a, b, a / b);
            break;
        case '%':
            Console.WriteLine("Result of {0} % {1} = {2}", a, b, a % b);
            break;
        default:
            Console.WriteLine("Wrong sign");
            break;

    }
}

    



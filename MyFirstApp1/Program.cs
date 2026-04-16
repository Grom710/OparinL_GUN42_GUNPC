using System;

class Program
{
    static void Main(string[] args)
    {
        
        Unit hero = new Unit("Герой");
        Unit enemy = new Unit();

       
        Console.WriteLine($"Создан юнит: {hero.Name}, Здоровье: {hero.Health}, Броня: {hero.Armor}, Урон: {hero.Damage}");
        Console.WriteLine($"Создан юнит: {enemy.Name}, Здоровье: {enemy.Health}, Броня: {enemy.Armor}, Урон: {enemy.Damage}");

       
        int attackDamage = 20;
        Console.WriteLine($"\n{hero.Name} наносит удар по {enemy.Name} с уроном {attackDamage}...");

        bool isDead = enemy.SetDamage(attackDamage);

        
        Console.WriteLine($"У {enemy.Name} осталось {enemy.Health:F2} здоровья.");
        Console.WriteLine($"Фактическое здоровье (с учётом брони): {enemy.GetRealHealth():F2}");

        if (isDead)
            Console.WriteLine($"{enemy.Name} погиб!");
        else
            Console.WriteLine($"{enemy.Name} выжил.");

      
        Console.WriteLine($"\n{hero.Name} наносит ещё один удар по {enemy.Name} с уроном {attackDamage}...");
        isDead = enemy.SetDamage(attackDamage);

        Console.WriteLine($"У {enemy.Name} осталось {enemy.Health:F2} здоровья.");

        if (isDead)
            Console.WriteLine($"{enemy.Name} погиб!");
        else
            Console.WriteLine($"{enemy.Name} выжил.");
    }
}







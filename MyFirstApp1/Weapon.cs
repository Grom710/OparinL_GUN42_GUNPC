using System;

public class Weapon
{
    private int minDamage;
    private int maxDamage;
    private readonly float durability;
    private readonly string name;

    public string Name => name;

    public int MinDamage
    {
        get => minDamage;
        private set => minDamage = value;
    }

    public int MaxDamage
    {
        get => maxDamage;
        private set => maxDamage = value;
    }

    public float Durability => durability;

    public Weapon(string name)
    {
        this.name = name;
        this.durability = 1f; 
        this.minDamage = 1;
        this.maxDamage = 10;
    }

    public Weapon(string name, int minDamage, int maxDamage) : this(name)
    {
        SetDamageParams(minDamage, maxDamage);
    }

    public void SetDamageParams(int minDamage, int maxDamage)
    {
        if (minDamage > maxDamage)
        {
            Console.WriteLine($"[{Name}] Некорректные входные данные: MinDamage > MaxDamage. Значения поменяны местами.");
            int temp = minDamage;
            minDamage = maxDamage;
            maxDamage = temp;
        }

        if (minDamage < 1)
        {
            Console.WriteLine($"[{Name}] Минимальный урон меньше 1. Установлено значение 1.");
            this.minDamage = 1;
        }
        else
        {
            this.minDamage = minDamage;
        }

        if (maxDamage <= 1)
        {
            Console.WriteLine($"[{Name}] Максимальный урон меньше или равен 1. Установлено значение 10.");
            this.maxDamage = 10;
        }
        else
        {
            this.maxDamage = maxDamage;
        }
    }

    public int GetDamage()
    {
        return (MinDamage + MaxDamage) / 2;
    }
}
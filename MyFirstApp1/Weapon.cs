using System;

public class Weapon
{
    private readonly string name;
    public Interval DamageRange { get; }

    public string Name => name;

    public Weapon(string name) : this(name, 1, 10) { }

    public Weapon(string name, int minDamage, int maxDamage)
    {
        this.name = name;

        this.DamageRange = new Interval(minDamage, maxDamage);
    }
}

public struct Room
{
    public Unit Unit { get; }
    public Weapon Weapon { get; }

    public Room(Unit unit, Weapon weapon)
    {
        Unit = unit;
        Weapon = weapon;
    }
}

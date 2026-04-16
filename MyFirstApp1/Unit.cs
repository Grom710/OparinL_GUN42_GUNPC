public class Unit
{
    private float health;
    private readonly float armor;
    private readonly string name;
    public Interval Damage { get; }

    public string Name => name;
    public float Health => health;
    public float Armor => armor;

    public Unit() : this("Unknown Unit", 0, 5) { }

    public Unit(string name, int minDamage, int maxDamage)
    {
        this.name = name;
        this.health = 100f; 
        this.armor = 0.6f;  

        this.Damage = new Interval(minDamage, maxDamage);
    }

    public bool TakeWeaponHit(Weapon weapon)
    {
        float damageTaken = weapon.DamageRange.Get() * armor;
        health -= damageTaken;
        Console.WriteLine($"{Name} получил {damageTaken:F2} урона от {weapon.Name}. Осталось здоровья: {health:F2}");
        return health <= 0f;
    }
}

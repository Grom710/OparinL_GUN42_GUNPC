public class Unit
{
    private float health;
    private readonly int damage;
    private readonly float armor;
    private readonly string name;

    public string Name => name;
    public float Health => health;
    public int Damage => damage;
    public float Armor => armor;

    public Unit() : this("Unknown Unit")
    {
    }

    public Unit(string name)
    {
        this.name = name;
        health = 100f; 
        damage = 5;
        armor = 0.6f;
    }

   
    public float GetRealHealth()
    {
        return health * (1f + armor);
    }

    
    public bool SetDamage(int value)
    {
        health -= value * armor;
        return health <= 0f;
    }
}
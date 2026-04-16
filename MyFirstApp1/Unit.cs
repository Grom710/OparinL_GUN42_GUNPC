public class Unit
{
    // Приватные поля
    private float health;
    private readonly int damage;
    private readonly float armor;
    private readonly string name;

    // Свойства только для чтения
    public string Name => name;
    public float Health => health;
    public int Damage => damage;
    public float Armor => armor;

    // Конструктор без аргументов, вызывает конструктор с аргументом
    public Unit() : this("Unknown Unit")
    {
    }

    // Конструктор с аргументом для имени
    public Unit(string name)
    {
        this.name = name;
        health = 100f; // Примерное начальное здоровье, можно изменить по задаче
        damage = 5;
        armor = 0.6f;
    }

    // Метод для получения фактического здоровья
    public float GetRealHealth()
    {
        return health * (1f + armor);
    }

    // Метод для получения урона
    public bool SetDamage(int value)
    {
        health -= value * armor;
        return health <= 0f;
    }
}
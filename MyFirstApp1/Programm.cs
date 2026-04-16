class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Добро пожаловать в Подземелье!");

        Dungeon dungeon = new Dungeon();

        dungeon.ShowRooms();

        Console.WriteLine("--- Симуляция боя ---");

        // Берем юнита из первой комнаты и оружие из второй для боя против юнита из третьей комнаты.
        Unit hero = dungeon.rooms[0].Unit;
        Weapon heroWeapon = dungeon.rooms[1].Weapon; // Герой берет трофейное оружие!
        Unit enemy = dungeon.rooms[2].Unit;

        Console.WriteLine($"\n{hero.Name} берет оружие {heroWeapon.Name} и атакует {enemy.Name}!");

        bool isEnemyDead = false;
        int turn = 1;

        while (!isEnemyDead && hero.Health > 0)
        {
            Console.WriteLine($"\n--- Ход {turn++} ---");
            isEnemyDead = enemy.TakeWeaponHit(heroWeapon);

            if (isEnemyDead)
            {
                Console.WriteLine($"\n{enemy.Name} повержен!");
            }
            else if (hero.Health <= 0)
            {
                Console.WriteLine($"\n{hero.Name} пал в бою...");
            }
            else
            {
                Console.WriteLine("Бой продолжается...");
            }
        }
    }
}
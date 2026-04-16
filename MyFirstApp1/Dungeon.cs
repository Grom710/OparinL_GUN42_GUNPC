public class Dungeon
{
    private readonly Room[] rooms;

    public Dungeon()
    {
        rooms = new Room[]
        {
            new Room(new Unit("Рыцарь", 15, 25), new Weapon("Меч Правосудия", 20, 40)),
            new Room(new Unit("Маг", 30, 40), new Weapon("Посох Архимага", 45, 60)),
            new Room(new Unit("Гоблин", 5, 15), new Weapon("Кинжал", 8, 18)),
            new Room(new Unit("Скелет", -5, -2), new Weapon("Ржавый меч", -10, -5)) 
        };

    }

    public void ShowRooms()
    {
        Console.WriteLine("=== Информация о комнатах в подземелье ===\n");

        for (int i = 0; i < rooms.Length; i++)
        {
            var room = rooms[i];

            Console.WriteLine($"--- Комната #{i + 1} ---");
            Console.WriteLine($"Юнит: {room.Unit.Name} | Здоровье: {room.Unit.Health} | Урон: {room.Unit.Damage.Min}-{room.Unit.Damage.Max}");
            Console.WriteLine($"Оружие: {room.Weapon.Name} | Урон: {room.Weapon.DamageRange.Min}-{room.Weapon.DamageRange.Max}");
            Console.WriteLine(new string('-', 25));
        }

        Console.WriteLine("=== Конец списка ===\n");
    }
}


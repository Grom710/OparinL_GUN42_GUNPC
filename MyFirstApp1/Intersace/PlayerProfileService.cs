// CasinoApp/Services/PlayerProfileService.cs
using System.IO;
using System.Text.Json;
using CasinoApp.Player;
using CasinoApp.Services; // Не забудь добавить using для нового интерфейса

namespace CasinoApp.Services
{
    // Класс реализует интерфейс, где T - это PlayerProfile
    public class PlayerProfileService : ISaveLoadService<PlayerProfile>
    {
        // Путь к файлу можно сделать константой или передавать через конструктор
        private const string BasePath = "data/";

        // Реализация метода из интерфейса ISaveLoadService
        public void SaveData(PlayerProfile data, string id)
        {
            // Убедимся, что папка data существует
            Directory.CreateDirectory(BasePath);

            string filePath = Path.Combine(BasePath, id + ".json");
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(data, options);
            File.WriteAllText(filePath, json);
            Console.WriteLine($"Данные сохранены в файл: {filePath}");
        }

        // Реализация метода из интерфейса ISaveLoadService
        public PlayerProfile LoadData(string id)
        {
            string filePath = Path.Combine(BasePath, id + ".json");

            if (!File.Exists(filePath))
                return null; // Или return default(PlayerProfile);

            try
            {
                string json = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<PlayerProfile>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при чтении файла: {ex.Message}");
                return null;
            }
        }

        // Теперь создадим удобные методы-обертки для основной программы

        public void SaveProfile(PlayerProfile profile)
        {
            // Вызываем реализацию интерфейса, передавая имя игрока как ID
            SaveData(profile, profile.Username);
        }

        public PlayerProfile LoadProfile()
        {
            // В данном случае ID нам неизвестен заранее при простом чтении,
            // поэтому этот метод требует доработки логики поиска файлов.
            // Для простоты примера оставим старый подход или изменим логику.

            // В текущем виде этот метод не может работать с новым интерфейсом напрямую,
            // так как ему нужен ID. Поэтому в Program.cs мы будем вызывать LoadData напрямую.

            // Этот метод можно удалить или оставить для совместимости,
            // но правильнее будет использовать LoadData в Program.cs.

            return null;
        }
    }
}
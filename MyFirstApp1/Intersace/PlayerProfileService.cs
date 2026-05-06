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

        public PlayerProfile LoadData(string id)
        {
            string filePath = Path.Combine(BasePath, id + ".json");

            if (!File.Exists(filePath))
                return null; 

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


        public void SaveProfile(PlayerProfile profile)
        {
            SaveData(profile, profile.Username);
        }

        public PlayerProfile LoadProfile()
        {

            return null;
        }
    }
}
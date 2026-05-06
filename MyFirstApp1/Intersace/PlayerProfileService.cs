using System.Text.Json;
using CasinoApp.Player;

namespace CasinoApp.Services
{
    public class PlayerProfileService : ISaveLoadService<PlayerProfile>
    {
        private const string BasePath = "data/";

        public void SaveData(PlayerProfile data, string id)
        {
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
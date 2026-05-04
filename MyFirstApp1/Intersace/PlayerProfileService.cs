// CasinoApp/Services/PlayerProfileService.cs
using System.IO;
using System.Text.Json;
using CasinoApp.Player;

namespace CasinoApp.Services
{
    public class PlayerProfileService
    {
        private const string ProfileFilePath = "player_profile.json";

        public void SaveProfile(PlayerProfile profile)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(profile, options);
            File.WriteAllText(ProfileFilePath, json);
        }

        public PlayerProfile LoadProfile()
        {
            if (!File.Exists(ProfileFilePath))
                return null;

            string json = File.ReadAllText(ProfileFilePath);
            return JsonSerializer.Deserialize<PlayerProfile>(json);
        }
    }
}
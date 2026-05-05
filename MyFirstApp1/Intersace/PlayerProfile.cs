// CasinoApp/Player/PlayerProfile.cs
namespace CasinoApp.Player
{
    public class PlayerProfile
    {
        public string Username { get; set; }
        public decimal Balance { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }

        public decimal CurrentBet { get; set; }
        public PlayerProfile(string username, decimal balance = 1000)
        {
            Username = username;
            Balance = balance;
            Wins = 0;
            Losses = 0;
            CurrentBet = 0; // Инициализируем нулем
        }
    }
}
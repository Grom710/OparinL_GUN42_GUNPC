using System;
using CasinoApp.CasinoApp;
using CasinoApp.Player;
using CasinoApp.Services;

namespace CasinoApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Загрузка профиля ===");

            PlayerProfileService profileService = new PlayerProfileService();

            string username = InputService.ReadString("Введите ваше имя для входа или регистрации: ");

            PlayerProfile player = profileService.LoadData(username);

            if (player == null)
            {
                Console.WriteLine($"Привет, {username}! Создаем новый профиль.");
                player = new PlayerProfile(username);
                profileService.SaveData(player, username);
                Console.WriteLine("Профиль успешно создан.");

                // Выдаем приветственный бонус новому игроку
                player.Balance = 2000;
                profileService.SaveData(player, username);
                Console.WriteLine($"Вам начислен приветственный бонус: {player.Balance} монет.");
            }

            // --- ГЛАВНОЕ ИЗМЕНЕНИЕ ---
            // Создаем объект Казино и запускаем игру через интерфейс
            IGame casinoManager = new Casino(player, profileService);
            casinoManager.StartGame();

            Console.WriteLine("Нажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}
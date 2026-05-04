// CasinoApp/Program.cs
using System;
using CasinoApp.Games.Blackjack;
using CasinoApp.Games.Dice;
using CasinoApp.Player;
using CasinoApp.Services;

namespace CasinoApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Создаем экземпляр сервиса. Он реализует ISaveLoadService<PlayerProfile>
            ISaveLoadService<PlayerProfile> profileService = new PlayerProfileService();

            string username;

            // 1. Запрашиваем имя игрока (ID)
            Console.Write("Введите ваше имя для входа/регистрации: ");
            username = Console.ReadLine().Trim();

            if (string.IsNullOrWhiteSpace(username))
            {
                Console.WriteLine("Имя не может быть пустым.");
                return;
            }

            // 2. Пробуем загрузить профиль по ID (имени)
            PlayerProfile player = profileService.LoadData(username);

            // 3. Если профиля нет (null), создаем новый и сразу сохраняем его
            if (player == null)
            {
                Console.WriteLine($"Привет, {username}! Создаем новый профиль.");
                player = new PlayerProfile(username);
                profileService.SaveData(player, username); // Сохраняем новый профиль
                Console.WriteLine("Профиль успешно создан.");
                player.Balance = 1500; // Можно выдать приветственный бонус новому игроку
                profileService.SaveData(player, username); // Сохраняем изменения с бонусом
            }

            ShowMenu(player, profileService);
        }

        static PlayerProfile CreateNewProfile(ISaveLoadService<PlayerProfile> service)
        {
            string username = InputService.ReadString("Введите имя игрока: ");
            PlayerProfile newPlayer = new(username);
            service.SaveData(newPlayer, username);
            Console.WriteLine($"Профиль создан. Баланс: {newPlayer.Balance}");
            return newPlayer;
        }

        static void ShowMenu(PlayerProfile player, ISaveLoadService<PlayerProfile> service)
        {
            while (true)
            {
                Console.WriteLine($"\n--- Казино ---");
                Console.WriteLine($"Игрок: {player.Username} | Баланс: {player.Balance} | Победы: {player.Wins} | Поражения: {player.Losses}");
                Console.WriteLine("1. Играть в Блэкджек");
                Console.WriteLine("2. Играть в Кости");
                Console.WriteLine("3. Сохранить и выйти");

                int choice = InputService.ReadInt("Ваш выбор: ", 1, 3);

                switch (choice)
                {
                    case 1:
                        new BlackjackGame().Play(player);
                        break;
                    case 2:
                        new DiceGame().Play(player);
                        break;
                    case 3:
                        // Сохраняем данные перед выходом через интерфейс
                        service.SaveData(player, player.Username);
                        Console.WriteLine("Профиль сохранён. До свидания!");
                        return;
                }
            }
        }
    }
}
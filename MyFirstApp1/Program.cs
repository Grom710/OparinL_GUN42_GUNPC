using CasinoApp.Games.Blackjack;
using CasinoApp.Games.Dice;
using CasinoApp.Player;
using CasinoApp.Services;
using MyFirstApp1.CasinoApp;
using System;

namespace CasinoApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Создаем экземпляры наших сервисов.
            // Профиль игрока сохраняется в папке "data/profiles"
            ISaveLoadService<PlayerProfile> profileService = new PlayerProfileService();

            // Универсальный сервис для работы с текстовыми файлами в папке "data/files"
            ISaveLoadService<string> fileSystemService = new FileSystemSaveLoadService("data/files");

            // --- БЛОК ВХОДА В ИГРУ ---
            Console.WriteLine("Добро пожаловать в Казино!");
            string username = InputService.ReadString("Введите ваше имя для входа или регистрации: ");

            if (string.IsNullOrWhiteSpace(username))
            {
                Console.WriteLine("Имя пользователя не может быть пустым. Выход из программы.");
                return;
            }

            // Пробуем загрузить существующий профиль по имени (ID)
            PlayerProfile player = profileService.LoadData(username);

            // Если профиля нет (player == null), создаем новый
            if (player == null)
            {
                Console.WriteLine($"Профиль для '{username}' не найден. Создаем новый.");
                player = new PlayerProfile(username);
                // Сразу сохраняем новый пустой профиль, чтобы файл создался
                profileService.SaveData(player, username);
                Console.WriteLine("Новый профиль успешно создан.");

                // Можно выдать приветственный бонус новому игроку
                player.Balance = 2000;
                profileService.SaveData(player, username); // Сохраняем бонус
                Console.WriteLine($"Вам начислен приветственный бонус: {player.Balance} монет.");
            }

            // Запускаем основное меню игры, передавая профиль и сервис для его сохранения
            ShowMenu(player, profileService);
        }

        /// <summary>
        /// Главное меню казино с выбором игр.
        /// </summary>
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
                        // Перед выходом обязательно сохраняем текущее состояние профиля
                        service.SaveData(player, player.Username);
                        Console.WriteLine("Профиль сохранён. До свидания!");
                        return; // Выходим из метода и завершаем программу
                }
            }
        }
    }
}
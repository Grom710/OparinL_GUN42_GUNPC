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
            // Создаем сервисы. Они создадут нужные папки при запуске.
            ISaveLoadService<PlayerProfile> profileService = new PlayerProfileService();
            ISaveLoadService<string> fileSystemService = new FileSystemSaveLoadService("data/files");

            // Переменная для хранения текущего игрока
            PlayerProfile player = null;

            // Флаг, чтобы управлять главным циклом программы
            bool isRunning = true;

            // ГЛАВНЫЙ ЦИКЛ ПРОГРАММЫ
            while (isRunning)
            {
                Console.WriteLine("\n--- Главное меню Казино ---");
                Console.WriteLine("1. Играть (Профиль игрока)");
                Console.WriteLine("2. Тест сервиса файлов (Сохранить/Загрузить текст)");
                Console.WriteLine("3. Выход");

                int mainChoice = InputService.ReadInt("Ваш выбор: ", 1, 3);

                switch (mainChoice)
                {
                    case 1: // --- БЛОК ИГРЫ ---
                        // Если игрок еще не вошел, просим его имя и загружаем профиль
                        if (player == null)
                        {
                            Console.Write("Введите ваше имя для входа или регистрации: ");
                            string username = Console.ReadLine().Trim();

                            if (string.IsNullOrWhiteSpace(username))
                            {
                                Console.WriteLine("Имя не может быть пустым.");
                                break; // Возвращаемся в главное меню
                            }

                            player = profileService.LoadData(username);

                            if (player == null)
                            {
                                Console.WriteLine($"Привет, {username}! Создаем новый профиль.");
                                player = new PlayerProfile(username);
                                profileService.SaveData(player, username);
                                Console.WriteLine("Профиль создан.");
                            }
                            else
                            {
                                Console.WriteLine($"С возвращением, {player.Username}!");
                            }
                        }
                        // Запускаем игровое меню, передавая профиль и сервис
                        ShowGameMenu(player, profileService);
                        break;

                    case 2: // --- БЛОК ТЕСТА ФАЙЛОВОГО СЕРВИСА ---
                        Console.WriteLine("\n--- Работа с текстовыми файлами ---");
                        Console.WriteLine("1. Сохранить текст");
                        Console.WriteLine("2. Загрузить текст");
                        int fileChoice = InputService.ReadInt("Выберите действие: ", 1, 2);

                        if (fileChoice == 1)
                        {
                            string textToSave = InputService.ReadString("Введите текст для сохранения: ");
                            string fileId = InputService.ReadString("Введите имя файла (без расширения): ");
                            fileSystemService.SaveData(textToSave, fileId);
                            Console.WriteLine("Текст успешно сохранен в файл data/files/" + fileId + ".txt");
                        }
                        else if (fileChoice == 2)
                        {
                            string fileId = InputService.ReadString("Введите имя файла для загрузки: ");
                            string loadedText = fileSystemService.LoadData(fileId);

                            if (loadedText != null)
                            {
                                Console.WriteLine("\n--- Содержимое файла ---");
                                Console.WriteLine(loadedText);
                                Console.WriteLine("----------------------");
                            }
                            else
                            {
                                Console.WriteLine("Файл не найден или пуст.");
                            }
                        }
                        break;

                    case 3: // --- ВЫХОД ---
                        isRunning = false;
                        Console.WriteLine("До свидания!");
                        break;
                }
            }
        }

        /// <summary>
        /// Меню с выбором игр (Блэкджек, Кости).
        /// </summary>
        static void ShowGameMenu(PlayerProfile player, ISaveLoadService<PlayerProfile> service)
        {
            bool inGameMenu = true;
            while (inGameMenu)
            {
                Console.WriteLine($"\n--- Игровое меню ---");
                Console.WriteLine($"Игрок: {player.Username} | Баланс: {player.Balance}");
                Console.WriteLine("1. Играть в Блэкджек");
                Console.WriteLine("2. Играть в Кости");
                Console.WriteLine("3. Назад в главное меню");

                int gameChoice = InputService.ReadInt("Ваш выбор: ", 1, 3);

                switch (gameChoice)
                {
                    case 1:
                        new BlackjackGame().Play(player);
                        service.SaveData(player, player.Username); // Сохраняем баланс после игры
                        break;
                    case 2:
                        new DiceGame().Play(player);
                        service.SaveData(player, player.Username); // Сохраняем баланс после игры
                        break;
                    case 3:
                        inGameMenu = false; // Возвращаемся в главное меню
                        break;
                }
            }
        }
    }
}
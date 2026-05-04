using System;
using CasinoApp.Player;
using CasinoApp.Services;

namespace CasinoApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Создаем сервисы для работы с данными.
            // Они автоматически создадут папки data/profiles и data/files при первом запуске.
            ISaveLoadService<PlayerProfile> profileService = new PlayerProfileService();
            ISaveLoadService<string> fileSystemService = new FileSystemSaveLoadService("data/files");

            // Переменная для хранения текущего игрока.
            PlayerProfile player = null;

            // Главный цикл программы.
            bool isRunning = true;
            while (isRunning)
            {
                Console.WriteLine("\n--- Главное меню Казино ---");
                Console.WriteLine("1. Играть");
                Console.WriteLine("2. Тест сервиса файлов");
                Console.WriteLine("3. Выход");

                int mainChoice = InputService.ReadInt("Ваш выбор: ", 1, 3);

                switch (mainChoice)
                {
                    case 1: // Блок входа в игру и выбора игры
                        // Если игрок еще не залогинен, просим его имя.
                        if (player == null)
                        {
                            string username = InputService.ReadString("Введите имя для регистрации или входа: ");
                            if (string.IsNullOrWhiteSpace(username))
                            {
                                Console.WriteLine("Имя не может быть пустым.");
                                break; // Возвращаемся в главное меню
                            }

                            // Пробуем загрузить профиль. Если не найден - создаем новый.
                            player = profileService.LoadData(username) ?? new PlayerProfile(username);
                            profileService.SaveData(player, username); // Сохраняем новый или обновляем существующий
                            Console.WriteLine($"Привет, {player.Username}! Ваш баланс: {player.Balance}");
                        }
                        // Показываем меню выбора игры (Блэкджек или Кости)
                        ShowGameSelectionMenu(player, profileService);
                        break;

                    case 2: // Блок тестирования файлового сервиса
                        TestFileSystemService(fileSystemService);
                        break;

                    case 3: // Выход из программы
                        isRunning = false;
                        Console.WriteLine("До свидания!");
                        break;
                }
            }
        }

        /// <summary>
        /// Меню выбора конкретной игры (Блэкджек или Кости).
        /// </summary>
        static void ShowGameSelectionMenu(PlayerProfile player, ISaveLoadService<PlayerProfile> service)
        {
            bool inMenu = true;
            while (inMenu)
            {
                Console.WriteLine($"\n--- Выбор игры ---");
                Console.WriteLine($"Игрок: {player.Username} | Баланс: {player.Balance}");
                Console.WriteLine("1. Играть в Блэкджек");
                Console.WriteLine("2. Играть в Кости");
                Console.WriteLine("3. Назад в главное меню");

                int gameChoice = InputService.ReadInt("Ваш выбор: ", 1, 3);

                switch (gameChoice)
                {
                    case 1:
                        StartBlackjack(player, service);
                        break;
                    case 2:
                        StartDice(player, service);
                        break;
                    case 3:
                        inMenu = false; // Возвращаемся в главное меню
                        break;
                }
            }
        }

        /// <summary>
        /// Запускает игру в Блэкджек с обработкой ошибок.
        /// </summary>
        static void StartBlackjack(PlayerProfile player, ISaveLoadService<PlayerProfile> service)
        {
            decimal bet = InputService.ReadDecimal("Ваша ставка в Блэкджек: ");
            
            try
            {
                // Создаем игру БЕЗ запуска сразу!
                var game = new CasinoApp.Games.Blackjack.BlackjackGame(player, bet);

                bool inPlayerTurnLoop = true;

                // Запускаем цикл взаимодействия с игроком здесь!
                while (inPlayerTurnLoop)
                {
                    // Просим пользователя сделать выбор в Program.cs
                    int choice = InputService.ReadInt("Ваш ход (1-Взять / 2-Стоп): ", 1, 2);

                    // Передаем выбор в игру через наш новый метод-шлюз
                    game.SetPlayerChoice(choice);

                    // Просим игру выполнить один шаг с этим выбором.
                    // Метод PlayOneStep будет выполнять логику одного хода.
                    inPlayerTurnLoop = game.PlayOneStep();

                    // Если игра закончилась (игрок перебрал или остановился), выходим из цикла.
                    if (!inPlayerTurnLoop || player.Balance <= 0)
                        break;
                }

                service.SaveData(player, player.Username);
            }
            catch (ArgumentException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Ошибка: {ex.Message}");
                Console.ResetColor();
            }
        }
        /// <summary>
        /// Запускает игру в Кости с обработкой ошибок.
        /// </summary>
        static void StartDice(PlayerProfile player, ISaveLoadService<PlayerProfile> service)
        {
            decimal bet = InputService.ReadDecimal("Ваша ставка в Кости: "); // Читаем ставку здесь

            try
            {
                int guess = InputService.ReadInt("Угадайте число (1-6): ", 1, 6); // Читаем угадываемое число здесь

                // Передаем все данные в конструктор игры
                var game = new CasinoApp.Games.Dice.DiceGame(player, bet, guess);

                service.SaveData(player, player.Username);
            }
            catch (ArgumentException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Ошибка: {ex.Message}");
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Тестовый метод для проверки работы FileSystemSaveLoadService.
        /// </summary>
        static void TestFileSystemService(ISaveLoadService<string> fileSystemService)
        {
            Console.WriteLine("\n--- Работа с текстовыми файлами ---");
            Console.WriteLine("1. Сохранить текст");
            Console.WriteLine("2. Загрузить текст");
            
            int fileChoice = InputService.ReadInt("Выберите действие: ", 1, 2);

            if (fileChoice == 1)
            {
                string textToSave = InputService.ReadString("Введите текст для сохранения: ");
                string fileId = InputService.ReadString("Введите имя файла (без расширения): ");
                
                fileSystemService.SaveData(textToSave, fileId);
                Console.WriteLine($"Текст успешно сохранен в файл data/files/{fileId}.txt");
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
        }
    }
}
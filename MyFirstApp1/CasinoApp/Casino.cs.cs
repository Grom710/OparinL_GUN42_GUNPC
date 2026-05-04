using System;
using CasinoApp.Games.Blackjack;
using CasinoApp.Games.Dice;
using CasinoApp.Player;
using CasinoApp.Services;

namespace CasinoApp
{
    namespace CasinoApp
    {
        public class Casino : IGame
        {
            private readonly PlayerProfile _player;
            private readonly ISaveLoadService<PlayerProfile> _profileService;

            public Casino(PlayerProfile player, ISaveLoadService<PlayerProfile> profileService)
            {
                _player = player;
                _profileService = profileService;
            }

            // Реализация метода из интерфейса IGame
            public void StartGame()
            {
                bool isInCasino = true;

                while (isInCasino)
                {
                    Console.WriteLine($"\n---=== Добро пожаловать в Казино, {_player.Username}! ===---");
                    Console.WriteLine($"Ваш баланс: {_player.Balance} | Победы: {_player.Wins} | Поражения: {_player.Losses}");
                    Console.WriteLine("1. Играть в Блэкджек");
                    Console.WriteLine("2. Играть в Кости");
                    Console.WriteLine("3. Проверить баланс и выйти");

                    int choice = InputService.ReadInt("Ваш выбор: ", 1, 3);

                    switch (choice)
                    {
                        case 1:
                            // --- ИСПРАВЛЕННАЯ СТРОКА ---
                            // Используем полное имя или просто имя, если есть using выше
                            StartBlackjackGame();
                            break;
                        case 2:
                            // --- ИСПРАВЛЕННАЯ СТРОКА ---
                            StartDiceGame();
                            break;
                        case 3:
                            // ...
                            Console.WriteLine("Сохраняем данные...");
                            _profileService.SaveData(_player, _player.Username);

                            Console.WriteLine("До свидания!");
                            return; // Выходим ИЗ МЕТОДА StartGame() полностью!

                            // Компилятор теперь счастлив, так как после return нет никакого кода.
                            // Нам больше не нужны ни 'isInCasino', ни 'break'.
                    }
                }
            }






            #region Методы запуска игр

            private void StartBlackjackGame()
            {
                decimal bet = GetBetFromUser();
                if (bet == 0) return;

                // --- ИСПРАВЛЕНИЕ ОШИБКИ ---
                // var blackjack = new Blackjack.BlackjackGame(...); // Ошибка!

                // Вариант 1 (рекомендуемый): Используем директиву using в начале файла
                // Теперь мы можем писать просто имя класса, так как мы "впустили" его в контекст.
                var blackjack = new BlackjackGame(_player, bet);

                // Вариант 2 (без using): Писать полное имя
                // var blackjack = new CasinoApp.Games.Blackjack.BlackjackGame(_player, bet);

                _profileService.SaveData(_player, _player.Username);
            }

            private void StartDiceGame()
            {
                decimal bet = GetBetFromUser();
                if (bet == 0) return;

                int diceCount = InputService.ReadInt("Сколько костей бросаем? (1-5): ", 1, 5);

                // --- ИСПРАВЛЕНИЕ ОШИБКИ ---
                // var diceGame = new Dice.DiceGame(...); // Ошибка!

                // Вариант 1 (с using в начале файла):
                var diceGame = new DiceGame(_player, bet, diceCount, 1, 6);

                // Вариант 2 (без using):
                // var diceGame = new CasinoApp.Games.Dice.DiceGame(_player, bet, diceCount, 1, 6);

                _profileService.SaveData(_player, _player.Username);
            }

            // Вспомогательный метод для получения ставки
            private decimal GetBetFromUser()
            {
                decimal bet = InputService.ReadDecimal("Ваша ставка: ");

                if (bet > _player.Balance)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Недостаточно средств для такой ставки.");
                    Console.ResetColor();
                    return 0; // Сигнал об отмене действия
                }
                return bet;
            }

            #endregion
        }
    }
}
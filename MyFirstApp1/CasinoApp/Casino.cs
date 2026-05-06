using System;
using CasinoApp.Games.Blackjack;
using CasinoApp.Games.Dice;
using CasinoApp.Player;
using CasinoApp.Services;

namespace CasinoApp
{
    public class Casino : IGame
    {
        private readonly PlayerProfile _player;
        private readonly ISaveLoadService<PlayerProfile> _profileService;

        private const decimal MaxBankValue = 3000;

        public Casino(PlayerProfile player, ISaveLoadService<PlayerProfile> profileService)
        {
            _player = player;
            _profileService = profileService;

            SubscribeToGameEvents();
        }

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
                        StartBlackjackGame();
                        break;
                    case 2:
                        StartDiceGame();
                        break;
                    case 3:
                        Console.WriteLine("Сохраняем данные...");
                        _profileService.SaveData(_player, _player.Username);
                        Console.WriteLine("До свидания!");
                        isInCasino = false; 
                        break;
                }
            }
        }

        #region Методы запуска игр

        private void StartBlackjackGame()
        {
            decimal bet = GetBetFromUser();
            if (bet == 0) return; 

            _player.CurrentBet = bet;

            var blackjack = new BlackjackGame(_player, bet);

            _profileService.SaveData(_player, _player.Username);

            _player.CurrentBet = 0;
        }

        private void StartDiceGame()
        {
            decimal bet = GetBetFromUser();
            if (bet == 0) return;

            int diceCount = InputService.ReadInt("Сколько костей бросаем? (1-5): ", 1, 5);

            _player.CurrentBet = bet;

            var diceGame = new DiceGame(_player, bet, diceCount, 1, 6);

            _profileService.SaveData(_player, _player.Username);
            _player.CurrentBet = 0;
        }

        #endregion

        #region Вспомогательные методы

        private decimal GetBetFromUser()
        {
            decimal bet = InputService.ReadDecimal("Ваша ставка: ");

            if (bet > _player.Balance)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Недостаточно средств для такой ставки.");
                Console.ResetColor();
                return 0; 
            }
            return bet;
        }

        private void SubscribeToGameEvents()
        { }


        #endregion

        #region Обработчики событий (Обновление профиля)

        public void UpdateProfileOnWin(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);

            decimal betAmount = _player.CurrentBet; 

            decimal newBalance = _player.Balance + (betAmount * 2);

            if (newBalance > MaxBankValue)
            {
                decimal overflowAmount = newBalance - MaxBankValue;
                _player.Balance = MaxBankValue;

                Console.WriteLine($"\n🎉🎉🎉 ВНИМАНИЕ! 🎉🎉🎉");
                Console.WriteLine($"Вы разорили казино! Ваш баланс ограничен: {MaxBankValue:N0}.");
                Console.WriteLine($"Излишек {overflowAmount:N2} пойдет на постройку нового заведения.");
                Console.WriteLine($"Ваш итоговый баланс: {_player.Balance:N0}");
            }
            else
            {
                _player.Balance = newBalance;
                Console.WriteLine($"Ваш баланс: {_player.Balance:N2}");
                Console.WriteLine($"(Ставка {betAmount} возвращена + выигрыш {betAmount})");
                _player.Wins++;
            }

            Console.ResetColor();
        }

        public void UpdateProfileOnLoose(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);

            decimal betAmount = _player.CurrentBet; 
            _player.Balance -= betAmount;
            Console.WriteLine($"Вы проиграли ставку: {betAmount}. Баланс: {_player.Balance:N2}");

            _player.Losses++;
            Console.ResetColor();
        }

        public void UpdateProfileOnDraw(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);

            decimal betAmount = _player.CurrentBet; 
            Console.WriteLine($"Ничья. Ставка {betAmount} возвращена. Баланс: {_player.Balance:N2}");

            Console.ResetColor();
        }

        #endregion
    }
}
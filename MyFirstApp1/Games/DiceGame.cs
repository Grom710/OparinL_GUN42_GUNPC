// CasinoApp/Games/Dice/DiceGame.cs

using System;
using CasinoApp.Player;
using CasinoApp.Games;
// using CasinoApp.Services; // Этот using здесь не нужен и даже вреден

namespace CasinoApp.Games.Dice
{
    public class DiceGame : CasinoGameBase
    {
        private readonly Random _random = new Random();
        private PlayerProfile _player;
        private decimal _bet;

        // 1. Создаем ЧАСТНОЕ ПОЛЕ для хранения угадываемого числа
        private int _guess;

        public DiceGame(PlayerProfile player, decimal bet, int guess)
        {
            // Проверки параметров (как и раньше)
            if (player == null)
                throw new ArgumentNullException(nameof(player), "Профиль игрока не может быть null.");
            if (bet <= 0)
                throw new ArgumentException("Ставка должна быть больше нуля.", nameof(bet));
            if (bet > player.Balance)
                throw new ArgumentException("Ставка превышает баланс игрока.", nameof(bet));

            _player = player;
            _bet = bet;

            // 2. Сохраняем пришедшее число в наше поле
            _guess = guess;

            this.OnWin += HandleWin;
            this.OnLoose += HandleLoose;
            this.OnDraw += HandleDraw;

            // Запускаем игру
            PlayGame();
        }

        protected override void FactoryMethod() { /* Логика */ }

        // 3. Метод PlayGame теперь БЕЗ ПАРАМЕТРОВ, как требует базовый класс!
        public override void PlayGame()
        {
            Console.WriteLine("--- Игра в Кости (Архитектура v2) ---");

            CasinoApp.Dice dice = new CasinoApp.Dice(1, 6);

            Console.WriteLine($"Бросок! Выпало число: {dice.Number}");

            // 4. Используем наше поле _guess вместо параметра
            if (_guess == dice.Number)
            {
                OnWinInvoke($"Угадали! Выпало {dice.Number}.");
            }
            else if (Math.Abs(_guess - dice.Number) == 1)
            {
                OnDrawInvoke($"Почти угадали! Разница всего в 1 очко.");
                _player.Balance += _bet;
            }
            else
            {
                OnLooseInvoke($"Не угадали. Выпало {dice.Number}.");
                _player.Balance -= _bet;
            }
        }


        // --- Приватные методы для обработки событий ---
        // Это разделение логики, как требовалось в ТЗ.

        private void HandleWin(string message)
        {
            Console.WriteLine(message);
            _player.Balance += _bet * 4;
            _player.Wins++;
            Console.WriteLine($"Ваш баланс: {_player.Balance}");
        }

        private void HandleLoose(string message)
        {
            Console.WriteLine(message);
            _player.Balance -= _bet;
            _player.Losses++;
            Console.WriteLine($"Ваш баланс: {_player.Balance}");
        }

        private void HandleDraw(string message)
        {
            Console.WriteLine(message);
            Console.WriteLine("Вам возвращается ставка.");
            Console.WriteLine($"Ваш баланс: {_player.Balance}");
        }
    }
}
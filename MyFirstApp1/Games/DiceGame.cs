using System;
using System.Collections.Generic;
using CasinoApp.Games; // Базовый абстрактный класс
using CasinoApp.Player; // Для профиля игрока
// using CasinoApp; // Не нужен, так как мы будем использовать псевдоним

// 1. РЕШЕНИЕ КОНФЛИКТА ИМЕН:
// Создаем псевдоним для структуры Dice, чтобы она не конфликтовала с пространством имен Games.Dice
using DiceType = CasinoApp.Dice;

namespace CasinoApp.Games.Dice
{
    public class DiceGame : CasinoGameBase
    {
        private readonly Random _random = new Random();
        private PlayerProfile _player;
        private decimal _bet;

        // 2. Коллекция для хранения костей (любая подходящая коллекция)
        private List<DiceType> _diceCollection;

        // Параметры для создания костей
        private int _diceCount;
        private int _minValue;
        private int _maxValue;

        // 3. ПУБЛИЧНЫЙ КОНСТРУКТОР (Требование 1)
        // Принимает количество костей и диапазон значений
        public DiceGame(PlayerProfile player, decimal bet, int diceCount, int minValue, int maxValue)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player), "Профиль игрока не может быть null.");
            if (bet <= 0 || bet > player.Balance)
                throw new ArgumentOutOfRangeException(nameof(bet), "Некорректная сумма ставки.");
            if (diceCount < 1)
                throw new ArgumentOutOfRangeException(nameof(diceCount), "Количество костей должно быть хотя бы 1.");

            _player = player;
            _bet = bet;
            _diceCount = diceCount;
            _minValue = minValue;
            _maxValue = maxValue;

            // Подписка на события для обновления профиля
            this.OnWin += UpdateProfileOnWin;
            this.OnLoose += UpdateProfileOnLoose;
            this.OnDraw += UpdateProfileOnDraw;

            PlayGame();
        }

        // 4. FACTORY METHOD (Требование 2)
        protected override void FactoryMethod()
        {
            _diceCollection = new List<DiceType>();

            for (int i = 0; i < _diceCount; i++)
            {
                try
                {
                    // Создаем кость с помощью псевдонима DiceType
                    DiceType dice = new DiceType(_minValue, _maxValue);
                    _diceCollection.Add(dice);
                }
                catch (CasinoApp.WrongDiceNumberException ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Ошибка при создании кости: {ex.Message}");
                    Console.ResetColor();
                    // Если параметры неверны, создаем кость по умолчанию 1-6
                    _diceCollection.Add(new DiceType(1, 6));
                }
            }

            Console.WriteLine($"Создано {_diceCollection.Count} костей с диапазоном {_minValue}-{_maxValue}.");
        }

        // 5. ОСНОВНАЯ МЕХАНИКА ИГРЫ (Требование 3)
        public override void PlayGame()
        {
            Console.WriteLine("\n---=== Игра в Кости ===---");

            FactoryMethod(); // Сначала создаем кости

            // Бросок Игрока и подсчет очков
            int playerSum = RollAllDice();
            OnPlayerRollInvoke($"Вы бросили кости. Сумма: {playerSum}");

            // Бросок Компьютера и подсчет очков
            int computerSum = RollAllDice();
            OnComputerRollInvoke($"Компьютер бросил кости. Сумма: {computerSum}");

            // Сравнение результатов и вызов событий
            if (playerSum > computerSum)
            {
                OnWinInvoke($"Победа! Ваша сумма ({playerSum}) больше, чем у компьютера ({computerSum}).");
                _player.Balance += _bet * 2; // Выигрыш: ставка удваивается
                _player.Wins++;
            }
            else if (playerSum < computerSum)
            {
                OnLooseInvoke($"Поражение. Ваша сумма ({playerSum}) меньше, чем у компьютера ({computerSum}).");
                _player.Balance -= _bet;
                _player.Losses++;
            }
            else
            {
                OnDrawInvoke($"Ничья! У вас и у компьютера по {playerSum} очков.");
                // При ничьей ставка возвращается (баланс не меняется)
                // Или можно сделать так: _player.Balance += _bet; 
            }

            Console.WriteLine($"Ваш баланс: {_player.Balance}");
        }


        #region Приватные методы механики

        /// <summary>
        /// "Бросает" все кости в коллекции и возвращает сумму очков.
        /// </summary>
        private int RollAllDice()
        {
            if (_diceCollection == null || _diceCollection.Count == 0)
            {
                Console.WriteLine("Коллекция костей пуста. Используем стандартные кости.");
                return _random.Next(_minValue, _maxValue + 1);
            }

            int sum = 0;
            foreach (var dice in _diceCollection)
            {
                // ИЗМЕНЕНИЕ:
                // Вместо того чтобы брать готовое число, мы заставляем кость "перекинуться"
                dice.Roll();

                // Теперь берем новое значение
                sum += dice.Number;
            }
            return sum;
        }

        #endregion

        #region Обработчики событий и вывод в консоль

        private void UpdateProfileOnWin(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        private void UpdateProfileOnLoose(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        private void UpdateProfileOnDraw(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        // Дополнительные методы для красивого вывода бросков
        private void OnPlayerRollInvoke(string message)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        private void OnComputerRollInvoke(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        #endregion
    }
}
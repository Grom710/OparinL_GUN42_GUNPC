using DiceType = CasinoApp.Dice;

using System;
using System.Collections.Generic;
using CasinoApp.Games; // Базовый абстрактный класс
using CasinoApp.Player; // Для профиля игрока
using CasinoApp; // Для доступа к константе MaxBankValue

namespace CasinoApp.Games.Dice
{
    public class DiceGame : CasinoGameBase
    {
        private readonly Random _random = new Random();
        private PlayerProfile _player;
        private decimal _bet;
        
        // Коллекция для хранения костей (требование задачи)
        private List<DiceType> _diceCollection;
        
        // Параметры костей из конструктора
        private int _diceCount;
        private int _minValue;
        private int _maxValue;
        
        // Лимит банка казино (требование задачи)
        private const decimal MaxBankValue = 3000;

        // Публичный конструктор (требование задачи)
        public DiceGame(PlayerProfile player, decimal bet, int diceCount, int minValue, int maxValue)
        {
            // Проверка на корректность входных параметров (требование задачи)
            if (player == null)
                throw new ArgumentNullException(nameof(player), "Профиль игрока не может быть null.");
            if (bet <= 0)
                throw new ArgumentOutOfRangeException(nameof(bet), "Ставка должна быть больше нуля.");
            if (bet > player.Balance)
                throw new ArgumentOutOfRangeException(nameof(bet), "Ставка превышает баланс игрока.");

            _player = player;
            _bet = bet;
            _diceCount = diceCount;
            _minValue = minValue;
            _maxValue = maxValue;

            // Подписываемся на события для обновления профиля игрока
            this.OnWin += UpdateProfileOnWin;
            this.OnLoose += UpdateProfileOnLoose;
            this.OnDraw += UpdateProfileOnDraw;

            // Запускаем игру сразу после создания объекта
            PlayGame();
        }

        // Реализация абстрактного метода FactoryMethod (требование задачи)
        protected override void FactoryMethod()
        {
            CreateAndShuffleDeck();
        }

        // Реализация абстрактного метода PlayGame (требование задачи)
        public override void PlayGame()
        {
            Console.WriteLine("\n---=== Игра в Кости ===---");
            
            FactoryMethod(); // 1. Создаем и тасуем кости

            // 2. Бросок Игрока и подсчет очков
            int playerSum = RollAllDice();
            OnPlayerRollInvoke($"Вы бросили кости. Сумма: {playerSum}");

            // 3. Бросок Компьютера и подсчет очков
            int computerSum = RollAllDice();
            OnComputerRollInvoke($"Компьютер бросил кости. Сумма: {computerSum}");

            // 4. Определение победителя и вызов событий
            if (playerSum > computerSum)
                OnWinInvoke($"Победа! Ваша сумма ({playerSum}) больше, чем у компьютера ({computerSum}).");
            
            else if (playerSum < computerSum)
                OnLooseInvoke($"Поражение. Ваша сумма ({playerSum}) меньше, чем у компьютера ({computerSum}).");
            
            else
                OnDrawInvoke($"Ничья! У вас и у компьютера по {playerSum} очков.");
        }


        #region Приватные методы механики

        // Реализация FactoryMethod: создание коллекции костей (требование задачи)
        private void CreateAndShuffleDeck()
        {
             _diceCollection = new List<DiceType>();
 
             for (int i = 0; i < _diceCount; i++)
             {
                 try
                 {
                     DiceType dice = new DiceType(_minValue, _maxValue);
                     _diceCollection.Add(dice);
                 }
                 catch (WrongDiceNumberException ex)
                 {
                     Console.ForegroundColor = ConsoleColor.Red;
                     Console.WriteLine($"Ошибка при создании кости: {ex.Message}");
                     Console.ResetColor();
                     // Создаем кость по умолчанию, если параметры неверны
                     _diceCollection.Add(new DiceType(1, 6));
                 }
             }
             
             Console.WriteLine($"Создано {_diceCollection.Count} костей с диапазоном {_minValue}-{_maxValue}.");
         }
        
         /// <summary>
         /// "Бросает" все кости в коллекции и возвращает сумму очков.
         /// Кости перекидываются при каждом вызове.
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
                  dice.Roll(); // Кость перекидывается!
                  sum += dice.Number; 
              }
              return sum;
          }
         
        #endregion

        #region Обработчики событий и сохранение

        private void UpdateProfileOnWin(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            
             // Механика "разорения казино" (требование задачи)
             decimal potentialNewBalance = _player.Balance + (_bet * 4); // В костях выигрыш x4

             if (potentialNewBalance > MaxBankValue)
             {
                 decimal overflowAmount = potentialNewBalance - MaxBankValue;
                 _player.Balance = MaxBankValue;
                 
                 Console.WriteLine($"\n🎉🎉🎉 ВНИМАНИЕ! 🎉🎉🎉");
                 Console.WriteLine($"Вы разорили казино! Ваш баланс ограничен: {MaxBankValue:N0}.");
                 Console.WriteLine($"Излишек {overflowAmount:N2} пойдет на постройку нового заведения.");
                 Console.WriteLine($"Ваш итоговый баланс: {_player.Balance:N0}");
             }
             else
             {
                 _player.Balance += _bet * 4; // Обычный выигрыш в костях x4
                 Console.WriteLine($"Ваш баланс: {_player.Balance:N2}");
                 _player.Wins++;
             }
             
             Console.ResetColor();
         }
         
         private void UpdateProfileOnLoose(string message)
         {
             Console.ForegroundColor = ConsoleColor.Red;
             Console.WriteLine(message);
             
             _player.Balance -= _bet;
             Console.WriteLine($"Ваш баланс: {_player.Balance:N2}");
             _player.Losses++;
             
             Console.ResetColor();
         }
         
         private void UpdateProfileOnDraw(string message)
         {
             Console.ForegroundColor = ConsoleColor.Yellow;
             Console.WriteLine(message);
             
             // В костях при "почти угадал" — это частичный возврат ставки.
             _player.Balance += _bet; 
             
             Console.WriteLine($"Ваш баланс: {_player.Balance:N2}");
             
             Console.ResetColor();
         }
         
         #endregion

     }
}
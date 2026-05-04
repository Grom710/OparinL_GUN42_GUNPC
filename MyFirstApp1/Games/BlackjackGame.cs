using CasinoApp; // Для структур Card, Suit, Rank
using CasinoApp.Games; // Базовый абстрактный класс
using CasinoApp.Player; // Профиль игрока
using CasinoApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CasinoApp.Games.Blackjack
{
    public class BlackjackGame : CasinoGameBase
    {
        // --- Поля класса ---
        private readonly Random _random = new Random();
        private PlayerProfile _player;
        private decimal _bet;
        private Queue<Card> _deck; // Колода карт
        private List<Card> _playerCards; // Карты игрока
        private List<Card> _dealerCards; // Карты дилера

        // --- Конструктор с проверкой параметров ---
        public BlackjackGame(PlayerProfile player, decimal bet)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player), "Профиль игрока не может быть null.");
            if (bet <= 0)
                throw new ArgumentOutOfRangeException(nameof(bet), "Ставка должна быть больше нуля.");
            if (bet > player.Balance)
                throw new ArgumentOutOfRangeException(nameof(bet), "Ставка превышает баланс игрока.");

            _player = player;
            _bet = bet;

            // Подписка на события для обновления профиля игрока
            this.OnWin += UpdateProfileOnWin;
            this.OnLoose += UpdateProfileOnLoose;
            this.OnDraw += UpdateProfileOnDraw;

            // Запускаем игру после успешной инициализации
            PlayGame();
        }

        // --- Реализация абстрактных методов из CasinoGameBase ---

        /// <summary>
        /// Фабричный метод. Здесь мы создаем и тасуем колоду.
        /// </summary>
        protected override void FactoryMethod()
        {
            CreateAndShuffleDeck();
        }

        /// <summary>
        /// Главный метод игры. Содержит всю механику раунда.
        /// </summary>
        public override void PlayGame()
        {
            Console.WriteLine("\n---=== Игра: Блэкджек ===---");

            FactoryMethod(); // 1. Создаем колоду

            // 2. Раздаем начальные карты
            _playerCards = new List<Card> { DrawCard(), DrawCard() };
            _dealerCards = new List<Card> { DrawCard(), DrawCard() };

            Console.WriteLine($"Ваши карты: {string.Join(", ", _playerCards)}");
            Console.WriteLine($"Карты дилера: {_dealerCards[0]}, *"); // Вторая карта дилера закрыта

            // 3. Ход игрока
            bool isPlayerTurn = true;
            while (isPlayerTurn && CalculateScore(_playerCards) <= 21)
            {
                Console.WriteLine($"\nВаши очки: {CalculateScore(_playerCards)}");
                Console.WriteLine("1 - Взять карту");
                Console.WriteLine("2 - Остановиться");

                int choice = InputService.ReadInt("Ваш выбор: ", 1, 2);

                if (choice == 1)
                {
                    var card = DrawCard();
                    _playerCards.Add(card);
                    Console.WriteLine($"Вы взяли карту: {card}");
                }
                else if (choice == 2)
                {
                    Console.WriteLine("Вы решили остановиться.");
                    isPlayerTurn = false;
                }
            }

            // 4. Проверка на перебор после хода игрока
            int playerSum = CalculateScore(_playerCards);
            if (playerSum > 21)
            {
                OnLooseInvoke("Перебор! У вас более 21 очка.");
                return; // Завершаем игру, если игрок проиграл сразу
            }

            // 5. Ход дилера (открываем карты)
            Console.WriteLine($"\nКарты дилера: {string.Join(", ", _dealerCards)}");
            while (CalculateScore(_dealerCards) < 17)
            {
                var card = DrawCard();
                _dealerCards.Add(card);
                Console.WriteLine($"Дилер берет карту: {card}");
            }

            // 6. Определение победителя
            int dealerSum = CalculateScore(_dealerCards);

            Console.WriteLine($"\nВаши очки: {playerSum}. Очки дилера: {dealerSum}.");

            if (dealerSum > 21 || playerSum > dealerSum)
            {
                OnWinInvoke("Вы победили! Поздравляем!");
                _player.Balance += _bet * 2; // Выигрыш (возврат ставки + выигрыш)
                _player.Wins++;
            }
            else if (playerSum == dealerSum)
            {
                OnDrawInvoke("Ничья! Ставка возвращается.");
                // Баланс не меняется, ставка просто возвращается игроку
                _player.Balance += _bet;
            }
            else
            {
                OnLooseInvoke("Вы проиграли.");
                _player.Balance -= _bet;
                _player.Losses++;
            }

            Console.WriteLine($"Ваш баланс после игры: {_player.Balance}");
        }


        // --- Приватные методы игровой механики ---

        /// <summary>
        /// Создает стандартную колоду из 36 карт и тасует ее.
        /// </summary>
        private void CreateAndShuffleDeck()
        {
            List<Card> cards = new List<Card>();

            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                for (int rankValue = (int)Rank.Six; rankValue <= (int)Rank.Ace; rankValue++)
                {
                    cards.Add(new Card(suit, (Rank)rankValue));
                }
            }

            _deck = new Queue<Card>(cards);
            ShuffleDeck();
        }

        /// <summary>
        /// Тасует текущую колоду.
        /// </summary>
        private void ShuffleDeck()
        {
            Card[] deckArray = _deck.ToArray();
            int n = deckArray.Length;

            while (n > 1)
            {
                n--;
                int k = _random.Next(n + 1);
                Card temp = deckArray[k];
                deckArray[k] = deckArray[n];
                deckArray[n] = temp;
            }

            _deck = new Queue<Card>(deckArray);
        }

        /// <summary>
        /// Достает одну карту из колоды.
        /// </summary>
        /// <returns>Объект карты</returns>
        private Card DrawCard()
        {
            if (_deck.Count == 0)
            {
                Console.WriteLine("\nКолода закончилась! Создаем и тасуем новую...");
                CreateAndShuffleDeck();
            }
            return _deck.Dequeue();
        }

        /// <summary>
        /// Подсчитывает сумму очков карт на руке с учетом туза.
        /// </summary>
        /// <param name="cards">Список карт</param>
        /// <returns>Сумма очков</returns>
        private int CalculateScore(List<Card> cards)
        {
            int score = cards.Sum(c => c.CardRank == Rank.Ace ? 11 : (int)c.CardRank);
            int aceCount = cards.Count(c => c.CardRank == Rank.Ace);

            while (score > 21 && aceCount > 0)
            {
                score -= 10; // Туз считается за 1 очко вместо 11
                aceCount--;
            }

            return score;
        }


        // --- Обработчики событий ---

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
    }
}
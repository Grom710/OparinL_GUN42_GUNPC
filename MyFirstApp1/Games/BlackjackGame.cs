using System;
using System.Collections.Generic;
using System.Linq;
using CasinoApp.Player;
using CasinoApp.Games;
using CasinoApp; // Для доступа к Card, Suit, Rank

namespace CasinoApp.Games.Blackjack
{
    public class BlackjackGame : CasinoGameBase
    {
        private readonly Random _random = new Random();
        private PlayerProfile _player;
        private decimal _bet;
        private Queue<Card> _deck;
        private List<Card> _playerCards;
        private List<Card> _dealerCards;

        // Поле для хранения выбора игрока (1 или 2)
        private int _playerChoice;

        public BlackjackGame(PlayerProfile player, decimal bet)
        {
            // Проверки параметров (как и раньше)
            if (player == null)
                throw new ArgumentNullException(nameof(player), "Игрок не может быть null.");
            if (bet <= 0 || bet > player.Balance)
                throw new ArgumentOutOfRangeException(nameof(bet), "Некорректная сумма ставки.");

            _player = player;
            _bet = bet;

            this.OnWin += UpdateProfileOnWin;
            this.OnLoose += UpdateProfileOnLoose;
            this.OnDraw += UpdateProfileOnDraw;

            PlayGame();
        }

        protected override void FactoryMethod()
        {
            CreateAndShuffleDeck();
        }

        public override void PlayGame()
        {
            Console.WriteLine("--- Блэкджек (Колода карт) ---");

            FactoryMethod();

            // Раздача начальных карт: 2 игроку, 2 дилеру
            _playerCards = new List<Card> { DrawCard(), DrawCard() };
            _dealerCards = new List<Card> { DrawCard(), DrawCard() };

            Console.WriteLine($"Ваши карты: {_playerCards[0]}, {_playerCards[1]}");
            Console.WriteLine($"Карты дилера: {_dealerCards[0]}, *");

            // --- ИЗМЕНЕННАЯ ЛОГИКА ХОДА ИГРОКА ---
            bool isPlayerTurn = true;

            // Цикл продолжается, пока игрок не остановится или не переберет
            while (isPlayerTurn && CalculateScore(_playerCards) <= 21)
            {
                // Выводим статус на экран
                Console.WriteLine($"\nВаши очки: {CalculateScore(_playerCards)}. Ваши карты: {string.Join(", ", _playerCards)}");
                Console.WriteLine("1 - Взять карту");
                Console.WriteLine("2 - Остановиться");

                // --- ВОТ ГЛАВНОЕ ИЗМЕНЕНИЕ ---
                // Мы НЕ вызываем InputService здесь!
                // Вместо этого, мы ждем, что выбор игрока уже будет в поле _playerChoice.

                // Вызываем метод, который просто выполняет действие по уже готовому выбору
                isPlayerTurn = PlayerTurn(_playerChoice);

                // --- КОНЕЦ ИЗМЕНЕНИЯ ---

                // После выполнения действия, поле _playerChoice нужно сбросить,
                // чтобы игра не зациклилась на одном и том же действии.
                _playerChoice = 0;
            }

            // Если игрок не перебрал, ход дилера
            if (CalculateScore(_playerCards) <= 21)
            {
                DealerTurn();
                DetermineWinner();
            }

            SaveGameResult();
        }

        // --- ИЗМЕНЕННЫЙ МЕТОД PlayerTurn ---
        // Он больше не спрашивает ввод. Он просто делает то, что ему сказали.
        private bool PlayerTurn(int choice)
        {
            if (choice == 1) // Взять карту
            {
                var card = DrawCard();
                _playerCards.Add(card);
                Console.WriteLine($"Вы взяли: {card}");
                return true; // Продолжаем ход игрока
            }
            else if (choice == 2) // Остановиться
            {
                Console.WriteLine("Вы решили остановиться.");
                return false; // Заканчиваем ход игрока
            }

            return false; // Если пришел странный выбор
        }


        #region Приватные методы механики

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
            Shuffle();
        }

        private void Shuffle()
        {
            Card[] deckArray = _deck.ToArray();

            for (int n = deckArray.Length - 1; n > 0; --n)
            {
                int k = _random.Next(n + 1);
                Card temp = deckArray[n];
                deckArray[n] = deckArray[k];
                deckArray[k] = temp;
            }

            _deck = new Queue<Card>(deckArray);
        }

        private Card DrawCard()
        {
            if (_deck.Count == 0)
            {
                Console.WriteLine("Колода закончилась! Тасуем заново.");
                CreateAndShuffleDeck();
            }
            return _deck.Dequeue();
        }

        private void DealerTurn()
        {
            Console.WriteLine($"\nКарты дилера: {_dealerCards[0]}, {_dealerCards[1]}");
            while (CalculateScore(_dealerCards) < 17)
            {
                var card = DrawCard();
                _dealerCards.Add(card);
                Console.WriteLine($"Дилер взял: {card}. Итого: {CalculateScore(_dealerCards)}");
            }
        }

        private void DetermineWinner()
        {
            int playerSum = CalculateScore(_playerCards);
            int dealerSum = CalculateScore(_dealerCards);

            Console.WriteLine($"\nИтог: У вас {playerSum}, у дилера {dealerSum}.");

            if (playerSum > 21 && dealerSum > 21)
            {
                OnDrawInvoke("Оба игрока перебрали. Ничья.");
            }
            else if (playerSum <= 21 && dealerSum > 21)
            {
                OnWinInvoke("Дилер перебрал. Вы победили!");
            }
            else if (dealerSum <= 21 && playerSum > 21)
            {
                OnLooseInvoke("Вы перебрали. Дилер победил.");
            }
            else if (playerSum == dealerSum && playerSum <= 21 && dealerSum <= 21)
            {
                OnDrawInvoke("Ничья. Одинаковое количество очков.");
            }
            else if (playerSum > dealerSum)
            {
                OnWinInvoke("У вас больше очков. Вы победили!");
            }
            else
            {
                OnLooseInvoke("У дилера больше очков. Вы проиграли.");
            }
        }

        #endregion

        #region Вспомогательные методы

        private int CalculateScore(List<Card> cards)
        {
            int score = cards.Sum(c => c.CardRank == Rank.Ace ? 11 : (int)c.CardRank);
            int aceCount = cards.Count(c => c.CardRank == Rank.Ace);

            while (score > 21 && aceCount > 0)
            {
                score -= 10;
                aceCount--;
            }
            return score;
        }

        #endregion

        #region Обработчики событий и сохранение

        private void UpdateProfileOnWin(string message)
        {
            _player.Balance += _bet * 2;
            _player.Wins++;
            Console.WriteLine(message);
            Console.WriteLine($"Баланс: {_player.Balance}");
        }

        private void UpdateProfileOnLoose(string message)
        {
            _player.Balance -= _bet;
            _player.Losses++;
            Console.WriteLine(message);
            Console.WriteLine($"Баланс: {_player.Balance}");
        }

        private void UpdateProfileOnDraw(string message)
        {
            Console.WriteLine(message);
            Console.WriteLine($"Баланс: {_player.Balance}"); // Ставка возвращается
        }

        private void SaveGameResult()
        {
            // Логика сохранения теперь в Program.cs. Здесь просто расчет.
        }

        // НОВЫЙ МЕТОД: Позволяет внешнему коду (Program.cs) передать выбор игрока в игру.
        public void SetPlayerChoice(int choice)
        {
            this._playerChoice = choice;
        }
    }
}
#endregion
// Обновляем using
using CasinoApp;
using CasinoApp.Games;
using CasinoApp.Player;
using CasinoApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CasinoApp.Games.Blackjack
{
    public class BlackjackGame : CasinoGameBase
    {
        private readonly Random _random = new Random();

        private PlayerProfile _player;
        private decimal _bet;

        private List<Card> _playerCards;
        private List<Card> _dealerCards;

        public BlackjackGame(PlayerProfile player, decimal bet)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player), "Игрок не задан.");

            if (bet <= 0 || bet > player.Balance)
                throw new ArgumentOutOfRangeException(nameof(bet), "Ставка вне допустимого диапазона.");

            _player = player;
            _bet = bet;

            this.OnWin += UpdateProfileOnWin;
            this.OnLoose += UpdateProfileOnLoose;

            PlayGame(); // Запускаем игру после инициализации
        }

        protected override void FactoryMethod()
        {
            Console.WriteLine("[Blackjack] Подготовка колоды...");
        }

        public override void PlayGame()
        {
            Console.WriteLine("--- Блэкджек (Архитектура v2) ---");

            InitializeRound();

            bool isPlayerTurn = true;
            while (isPlayerTurn)
            {
                DisplayRoundStatus();
                int choice = InputService.ReadInt("1 - Взять карту | 2 - Остановиться: ", 1, 2);

                if (choice == 1) HitPlayer();

                if (CalculateScore(_playerCards) > 21)
                {
                    OnLooseInvoke("Перебор у игрока!");
                    return; // Конец игры
                }

                if (choice == 2) isPlayerTurn = false;
            }

            DealerTurn();
            DetermineWinner();
        }

        #region Приватные методы механики (Разделение логики по ТЗ)

        private void InitializeRound()
        {
            _playerCards = new List<Card> { DrawCard(), DrawCard() };
            _dealerCards = new List<Card> { DrawCard(), DrawCard() };
        }

        private void DisplayRoundStatus()
        {
            Console.WriteLine($"\nВаши карты: {string.Join(", ", _playerCards)} (Очки: {CalculateScore(_playerCards)})");
            Console.WriteLine($"Карты дилера: {_dealerCards[0]}, *");
        }

        private void HitPlayer()
        {
            var card = DrawCard();
            _playerCards.Add(card);
            Console.WriteLine($"Вы взяли: {card}");
        }

        private void DealerTurn()
        {
            Console.WriteLine($"\nКарты дилера: {string.Join(", ", _dealerCards)}");
            while (CalculateScore(_dealerCards) < 17)
            {
                var card = DrawCard();
                _dealerCards.Add(card);
                Console.WriteLine($"Дилер взял: {card}");
                Console.WriteLine($"Итого у дилера: {CalculateScore(_dealerCards)}");
            }
        }

        private void DetermineWinner()
        {
            int playerSum = CalculateScore(_playerCards);
            int dealerSum = CalculateScore(_dealerCards);

            Console.WriteLine($"\nИтог: У вас {playerSum}, у дилера {dealerSum}.");

            if (dealerSum > 21 || playerSum > dealerSum)
                OnWinInvoke("Вы выиграли! Дилер перебрал или у вас больше очков.");

            else if (playerSum == dealerSum)
                OnDrawInvoke("Ничья!");

            else
                OnLooseInvoke("Вы проиграли. У дилера больше очков.");
        }

        #endregion

        #region Вспомогательные методы

        private Card DrawCard() => new Card(GetRandomSuit(), GetRandomRank());
        private Suit GetRandomSuit() => (Suit)_random.Next(0, 4);
        private Rank GetRandomRank() => (Rank)_random.Next(6, 15);

        private int CalculateScore(List<Card> cards)
        {
            int score = cards.Sum(c => c.CardRank == Rank.Ace ? 11 : (int)c.CardRank);
            int aceCount = cards.Count(c => c.CardRank == Rank.Ace);

            while (score > 21 && aceCount > 0)
            {
                score -= 10; // Туз с 11 превращается в 1
                aceCount--;
            }
            return score;
        }

        #endregion

        #region Обработчики событий (Обновление профиля)

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

        #endregion
    }
}
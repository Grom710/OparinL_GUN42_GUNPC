// Обновляем using и добавляем CasinoApp для структур Card и Suit/Rank
using System;
using System.Collections.Generic;
using System.Linq;
using CasinoApp.Player;
using CasinoApp.Services;
using CasinoApp;

namespace CasinoApp.Games.Blackjack
{
    public class BlackjackGame
    {
        private readonly Random _random = new Random();

        // Метод для получения случайной масти
        private Suit GetRandomSuit() => (Suit)_random.Next(0, 4);

        // Метод для получения случайной величины карты (от Six до Ace)
        private Rank GetRandomRank() => (Rank)_random.Next(6, 15); // 6 - это Six, 14 - это Ace

        // Метод для подсчета очков с учетом "гибкости" Туза
        private int CalculateScore(List<Card> cards)
        {
            int score = 0;
            int aceCount = 0;

            foreach (var card in cards)
            {
                switch (card.CardRank)
                {
                    case Rank.Jack:
                    case Rank.Queen:
                    case Rank.King:
                        score += 10;
                        break;
                    case Rank.Ace:
                        score += 11; // Считаем Туза за 11 пока
                        aceCount++;
                        break;
                    default:
                        score += (int)card.CardRank; // Для Six(6) ... Ten(10) работает напрямую
                        break;
                }
            }

            // Если перебор и есть тузы, превращаем их из 11 в 1 пока не перестанем перебирать
            while (score > 21 && aceCount > 0)
            {
                score -= 10; // Было 11, стало 1 (разница 10)
                aceCount--;
            }

            return score;
        }

        public void Play(PlayerProfile player)
        {
            Console.WriteLine("--- Блэкджек (21) ---");
            decimal bet = InputService.ReadDecimal("Ваша ставка: ");
            if (bet > player.Balance)
            {
                Console.WriteLine("Недостаточно средств.");
                return;
            }

            List<Card> playerCards = new List<Card>
             {
                 new Card(GetRandomSuit(), GetRandomRank()),
                 new Card(GetRandomSuit(), GetRandomRank())
             };

            List<Card> dealerCards = new List<Card>
             {
                 new Card(GetRandomSuit(), GetRandomRank()),
                 new Card(GetRandomSuit(), GetRandomRank())
             };

            Console.WriteLine($"Ваши карты: {string.Join(", ", playerCards)}");
            Console.WriteLine($"Карты дилера: {dealerCards[0]}, *");

            bool isPlayerTurn = true;
            while (isPlayerTurn)
            {
                Console.WriteLine("\nВаш ход:");
                Console.WriteLine("1 - Взять карту");
                Console.WriteLine("2 - Остановиться");

                int choice = InputService.ReadInt("Ваш выбор: ", 1, 2);

                switch (choice)
                {
                    case 1:
                        playerCards.Add(new Card(GetRandomSuit(), GetRandomRank()));
                        Console.WriteLine($"Вы взяли карту. Ваши карты: {string.Join(", ", playerCards)}");

                        if (CalculateScore(playerCards) > 21)
                        {
                            Console.WriteLine("Перебор! Вы проиграли.");
                            player.Balance -= bet;
                            player.Losses++;
                            return; // Завершаем игру немедленно при переборе у игрока
                        }
                        break;
                    case 2:
                        isPlayerTurn = false;
                        break;
                }
            }

            Console.WriteLine($"\nКарты дилера: {string.Join(", ", dealerCards)}");

            // Ход дилера: берет карты, пока у него меньше 17 очков.
            while (CalculateScore(dealerCards) < 17)
            {
                dealerCards.Add(new Card(GetRandomSuit(), GetRandomRank()));
                Console.WriteLine($"Дилер взял карту. Теперь у него: {string.Join(", ", dealerCards)}");
            }

            int playerSum = CalculateScore(playerCards);
            int dealerSum = CalculateScore(dealerCards);

            Console.WriteLine($"\nВаши очки: {playerSum}. Очки дилера: {dealerSum}.");

            if (dealerSum > 21 || playerSum > dealerSum)
            {
                player.Balance += bet * 2;
                player.Wins++;
                Console.WriteLine($"Вы выиграли! Ваш баланс: {player.Balance}");
            }
            else if (playerSum == dealerSum)
            {
                Console.WriteLine($"Ничья. Ваш баланс: {player.Balance}");
            }
            else
            {
                player.Balance -= bet;
                player.Losses++;
                Console.WriteLine($"Вы проиграли. Ваш баланс: {player.Balance}");
            }
        }
    }
}
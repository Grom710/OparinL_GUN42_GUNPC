// CasinoApp/Games/Blackjack/BlackjackGame.cs
using System;
using System.Collections.Generic;
using CasinoApp.Player;
using CasinoApp.Services;

namespace CasinoApp.Games.Blackjack
{
    public class BlackjackGame
    {
        private readonly Random _random = new();

        // Метод DrawCard остается прежним
        private int DrawCard() => _random.Next(1, 12); // Карты от 1 до 11

        public void Play(PlayerProfile player)
        {
            Console.WriteLine("--- Блэкджек (21) ---");
            decimal bet = InputService.ReadDecimal("Ваша ставка: ");
            if (bet > player.Balance)
            {
                Console.WriteLine("Недостаточно средств.");
                return;
            }

            List<int> playerCards = new() { DrawCard(), DrawCard() };
            List<int> dealerCards = new() { DrawCard(), DrawCard() };

            Console.WriteLine($"Ваши карты: {string.Join(", ", playerCards)} (Сумма: {SumCards(playerCards)})");
            Console.WriteLine($"Карты дилера: {dealerCards[0]}, *");

            // --- ИЗМЕНЕННЫЙ ЦИКЛ ХОДА ИГРОКА ---
            bool isPlayerTurn = true;
            while (isPlayerTurn)
            {
                // Выводим меню с вариантами 1 и 2
                Console.WriteLine("\nВаш ход:");
                Console.WriteLine("1 - Взять карту");
                Console.WriteLine("2 - Остановиться");

                int choice = InputService.ReadInt("Ваш выбор: ", 1, 2);

                switch (choice)
                {
                    case 1: // Взять карту
                        playerCards.Add(DrawCard());
                        Console.WriteLine($"Вы взяли карту. Ваши карты: {string.Join(", ", playerCards)} (Сумма: {SumCards(playerCards)})");

                        // Проверка на перебор (больше 21)
                        if (SumCards(playerCards) > 21)
                        {
                            Console.WriteLine("Перебор! Вы проиграли.");
                            player.Balance -= bet;
                            player.Losses++;
                            return; // Завершаем игру
                        }
                        break;

                    case 2: // Остановиться
                        Console.WriteLine("Вы решили остановиться.");
                        isPlayerTurn = false; // Выходим из цикла хода игрока
                        break;
                }
            }

            // --- ХОД ДИЛЕРА (остается без изменений) ---
            Console.WriteLine($"\nКарты дилера: {string.Join(", ", dealerCards)}");
            while (SumCards(dealerCards) < 17)
            {
                dealerCards.Add(DrawCard());
                Console.WriteLine($"Дилер взял карту. Теперь у него: {string.Join(", ", dealerCards)}");
            }

            // --- ПОДСЧЕТ РЕЗУЛЬТАТА ---
            int playerSum = SumCards(playerCards);
            int dealerSum = SumCards(dealerCards);

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

        private int SumCards(List<int> cards) => cards.Sum();
    }
}
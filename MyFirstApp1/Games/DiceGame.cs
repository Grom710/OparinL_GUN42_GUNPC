// CasinoApp/Games/Dice/DiceGame.cs
using System;
using CasinoApp.Player;
using CasinoApp.Services;

namespace CasinoApp.Games.Dice
{
    public class DiceGame
    {
        private readonly Random _random = new();

        public void Play(PlayerProfile player)
        {
            Console.WriteLine("--- Игра в Кости ---");
            decimal bet = InputService.ReadDecimal("Ваша ставка: ");
            if (bet > player.Balance)
            {
                Console.WriteLine("Недостаточно средств.");
                return;
            }

            int guess = InputService.ReadInt("Угадайте сумму двух костей (2-12): ", 2, 12);
            int dice1 = _random.Next(1, 7);
            int dice2 = _random.Next(1, 7);
            int sum = dice1 + dice2;

            Console.WriteLine($"Выпало: {dice1} + {dice2} = {sum}");

            if (guess == sum)
            {
                player.Balance += bet * 4;
                player.Wins++;
                Console.WriteLine($"Угадали! Ваш баланс: {player.Balance}");
            }
            else if (Math.Abs(guess - sum) == 1)
            {
                player.Balance += bet * 2;
                player.Wins++;
                Console.WriteLine($"Почти угадали! Ваш баланс: {player.Balance}");
            }
            else
            {
                player.Balance -= bet;
                player.Losses++;
                Console.WriteLine($"Не угадали. Ваш баланс: {player.Balance}");
            }
        }
    }
}
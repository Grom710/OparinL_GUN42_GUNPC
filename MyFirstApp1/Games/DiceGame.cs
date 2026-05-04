// Заменяем старые using на новые, включая наше пространство имен с CardAndDice
using System;
using System.Collections.Generic;
using CasinoApp.Player;
using CasinoApp.Services;


namespace CasinoApp.Games.Dice
{
    public class DiceGame
    {
        private readonly Random _random = new Random();

        // Метод Play теперь использует новую структуру Dice для валидации ставок и бросков
        public void Play(PlayerProfile player)
        {
            Console.WriteLine("--- Игра в Кости (с проверкой) ---");

            decimal bet = InputService.ReadDecimal("Ваша ставка: ");
            if (bet > player.Balance)
            {
                Console.WriteLine("Недостаточно средств.");
                return;
            }

            int guess;
            while (true)
            {
                guess = InputService.ReadInt("Угадайте число (1-6): ", 1, 6);

                // Проверка диапазона ставки относительно баланса игрока
                if (bet * 4 > player.Balance && guess == 6) // Пример сложной логики, если нужна
                {
                    Console.WriteLine("Ставка слишком высока для такого выигрыша.");
                    continue;
                }
                break;
            }

            try
            {
                // Создаем экземпляр структуры Dice с диапазоном 1-6.
                // Если бы мы передали неверные числа, вылетело бы исключение.
                CasinoApp.Dice dice = new CasinoApp.Dice(1, 6);

                Console.WriteLine($"Бросок! Выпало число: {dice.Number}");

                if (guess == dice.Number)
                {
                    player.Balance += bet * 4;
                    player.Wins++;
                    Console.WriteLine($"Угадали! Ваш баланс: {player.Balance}");
                }
                else if (Math.Abs(guess - dice.Number) == 1)
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
            catch (WrongDiceNumberException ex)
            {
                // Обработка нашей ошибки (хотя в игре она не должна возникнуть)
                Console.WriteLine($"Критическая ошибка игры: {ex.Message}");
            }
        }
    }
}
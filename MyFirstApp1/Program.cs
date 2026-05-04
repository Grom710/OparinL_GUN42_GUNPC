// CasinoApp/Program.cs
using System;
using CasinoApp.Games.Blackjack;
using CasinoApp.Games.Dice;
using CasinoApp.Player;
using CasinoApp.Services;

namespace CasinoApp
{
    class Program
    {
        // В методе Main
        // В методе Main
        static void Main(string[] args)
        {
            PlayerProfileService profileService = new();
            PlayerProfile player;

            // Пробуем загрузить. Если вернет null - создаем новый.
            if ((player = profileService.LoadProfile()) == null)
            {
                player = CreateNewProfile(profileService);
            }

            ShowMenu(player, profileService);
        }

        static PlayerProfile CreateNewProfile(PlayerProfileService service)
        {
            string username = InputService.ReadString("Введите имя игрока: ");
            PlayerProfile newPlayer = new(username);
            service.SaveProfile(newPlayer);
            Console.WriteLine($"Профиль создан. Баланс: {newPlayer.Balance}");

            return newPlayer; // <-- Вот эта строка возвращает объект
        }

        static void ShowMenu(PlayerProfile player, PlayerProfileService service)
         {
             while (true)
             {
                 Console.WriteLine($"\n--- Казино ---");
                 Console.WriteLine($"Игрок: {player.Username} | Баланс: {player.Balance} | Победы: {player.Wins} | Поражения: {player.Losses}");
                 Console.WriteLine("1. Играть в Блэкджек");
                 Console.WriteLine("2. Играть в Кости");
                 Console.WriteLine("3. Сохранить и выйти");
                 
                 int choice = InputService.ReadInt("Ваш выбор: ", 1, 3);
                 
                 switch (choice)
                 {
                     case 1:
                         new BlackjackGame().Play(player);
                         break;
                     case 2:
                         new DiceGame().Play(player);
                         break;
                     case 3:
                         service.SaveProfile(player);
                         Console.WriteLine("Профиль сохранён. До свидания!");
                         return;
                 }
             }
         }
     }
 }
// <copyright file="MainMenuActions.cs" company="DnamSolutions">
// Copyright (c) DnamSolutions. All rights reserved.
// </copyright>

namespace TidyingDesktop.UI.Menus.MainMenu
{
    using System;
    using TidyingDesktop.StaticClasses;
    using TidyingDesktop.UI;
    using TidyingDesktop.UI.Menus;
    using TidyingDesktop.UI.Menus.ConfigMenu;

    /// <summary>
    /// Represents a method with a <see cref="string"/> parameter and <see cref="void"/> return.
    /// </summary>
    /// <param name="msg">The string parameter for a method.</param>
    public delegate void NotifyAction(string msg);

    /// <summary>
    /// Represents the actions of the Main Menu.
    /// </summary>
    internal static class MainMenuActions
    {
        private static readonly string[] ActionNames = new string[]
        {
            "Order",
            "Restore BackUp",
            "Configuration",
        }
        .Select(s => s.ToUpper()).ToArray();

        private static List<SingleAction> actions;

        static MainMenuActions()
        {
            actions = new List<SingleAction>
            {
                new SingleAction(ActionNames[0], OrderAction),
                new SingleAction(ActionNames[1], RestoreBackUp),
            };
        }

        /// <summary>
        /// Gets the actions of the Main Menu.
        /// </summary>
        public static List<SingleAction> Actions
        {
            get
            {
                return actions;
            }
        }

        private static void OrderAction()
        {
            Console.WriteLine($"Folder to Order: {DataOperations.Configuration.OriginDirectoryPath}");
            Console.WriteLine($"Destination: {DataOperations.Configuration.DestinationDirectoryPath}");
            Console.WriteLine("Proceed? (Y/N)");
            ConsoleKey key = Console.ReadKey().Key;
            Console.Clear();

            if (key == ConsoleKey.Y)
            {
                Console.Clear();
                try
                {
                    DataOperations.OrderFiles();
                    DataOperations.MakeBackUp();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
            else
            {
                Console.WriteLine("Operation cancelled");
            }
        }

        private static void RestoreBackUp()
        {
            try
            {
                ConsoleKey consoleKey;
                Console.WriteLine("Restore last Backup?");
                Console.WriteLine($"From: {DataOperations.RestoredBackup.DestinationDirectory} To: {DataOperations.RestoredBackup.OriginDirectory}");
                Console.WriteLine("(Y/N)");
                consoleKey = Console.ReadKey().Key;
                Console.Clear();

                if (consoleKey == ConsoleKey.Y)
                {
                    DataOperations.RestoreBackup();
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("No backup founded.");
            }
        }
    }
}

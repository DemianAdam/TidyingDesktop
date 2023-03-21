// <copyright file="MenuUI.cs" company="DnamSolutions">
// Copyright (c) DnamSolutions. All rights reserved.
// </copyright>

namespace TidyingDesktop.UI.Menus
{
    using System.Globalization;
    using TidyingDesktop.StaticClasses;

    /// <summary>
    /// Represents UI elements of temp DisplayMenu.
    /// </summary>
    internal static class MenuUI
    {
        private static TextInfo ti = CultureInfo.CurrentCulture.TextInfo;

        private static bool exit = false;
        private static bool exitShowed = false;

        /// <summary>
        /// Display a Menu with choices.
        /// </summary>
        /// <param name="menu">Represents the parentMenu .</param>
        public static void DisplayMenu(IMenuConfig menu)
        {
            int choice;
            int backChoice = 0;
            int exitChoice = 0;
            int cChoices = menu.MenuChoices.Length;
            do
            {
                do
                {
                    Console.WriteLine($"Folder to Order: {DataOperations.Configuration.OriginDirectoryPath} ---> Destination: {DataOperations.Configuration.DestinationDirectoryPath}");
                    Console.WriteLine($"Include folders: {DataOperations.Configuration.IncludeFolders}\n");
                    Console.WriteLine(ti.ToTitleCase(menu.Name.ToLower()));
                    cChoices = menu.MenuChoices.Length;
                    for (int i = 0; i < cChoices; i++)
                    {
                        if (menu.MenuChoices[i].ToUpper() == "BACK" || menu.MenuChoices[i].ToUpper() == "EXIT")
                        {
                            i--;
                            continue;
                        }

                        DisplayChoice(i, menu.MenuChoices[i]);
                    }

                    if (menu.ParentMenu is not null)
                    {
                        DisplayChoice(cChoices, "Back");
                        backChoice = cChoices++;
                    }

                    DisplayChoice(cChoices, "Exit");
                    exitChoice = cChoices += 1;

                    choice = UserChoice(cChoices);
                    Console.Clear();
                }
                while (choice <= 0 || choice >= cChoices + 2);

                if (exitChoice == choice)
                {
                    exit = true;
                }
                else if (choice == backChoice + 1 && menu.ParentMenu is not null)
                {
                    DisplayMenu(menu.ParentMenu);
                }
                else
                {
                    string sChoice = menu.MenuChoices[choice - 1];
                    SingleAction? action = menu.MenuActions?.Find(a => a.ActionName == sChoice.ToUpper());

                    if (menu.MenuActions is not null && action is not null)
                    {
                        menu.Action(sChoice);
                    }
                    else if (menu.ChildMenus is not null)
                    {
                        IMenuConfig? nullableMenu = menu.ChildMenus.Find(x => x.Name == sChoice.ToUpper());

                        if (nullableMenu is not null)
                        {
                            DisplayMenu(nullableMenu);
                        }
                    }
                }
            }
            while ((choice != cChoices + 1) && !exit);

            if (exit == true && exitShowed == false)
            {
                exitShowed = true;
                AskForExit();
            }
        }

        /// <summary>
        /// Waits to the user to chose temp value greater than 0 and smaller to <paramref name="cChoice"/>.
        /// </summary>
        /// <param name="cChoice">The quantity to evaluate.</param>
        /// <returns>
        ///     <list type="bullet">
        ///         <item>If the value does not met the condition, returns 0.</item>
        ///         <item>If the value is a string, return -1.</item>
        ///         <item>If the value is a null or empty string, return -2.</item>
        ///     </list>
        /// </returns>
        public static int UserChoice(int cChoice)
        {
            int choice = -1;
            string? sChoice = Console.ReadLine();
            if (int.TryParse(sChoice, out choice))
            {
                if (choice <= cChoice && choice > 0)
                {
                    return choice;
                }
            }

            if (!string.IsNullOrEmpty(sChoice))
            {
                return -1;
            }

            if (string.IsNullOrEmpty(sChoice))
            {
                return -2;
            }

            return choice;
        }

        /// <summary>
        /// Shows a Pause message in the console.
        /// </summary>
        public static void PauseMessage()
        {
            Console.WriteLine("Type anything to continue...");
            Console.ReadKey();
            Console.Clear();
        }

        /// <summary>
        /// Displays the loaded Formats and the actual Formats.
        /// </summary>
        public static void DisplayFormats()
        {
            Console.WriteLine("--------------------");
            Console.WriteLine("LOADED Formats: \n");
            foreach (var item in DataOperations.SessionTypes)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine("--------------------");
            Console.WriteLine("ACTUAL Formats: \n");
            foreach (var item in DataOperations.Types)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine("--------------------");
        }

        /// <summary>
        /// Displays the loaded Extension-Formats and the actual Extension-Formats.
        /// </summary>
        public static void DisplayExtensionFormats()
        {
            Console.WriteLine("--------------------");
            Console.WriteLine("Extension-Formats LOADED: \n");
            foreach (var item in DataOperations.SessionData)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine("--------------------");
            Console.WriteLine("Extension-Formats ACTUAL: \n");
            foreach (var item in DataOperations.ExtensionTypePair)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine("--------------------");
        }

        /// <summary>
        /// Displays a choice in the console.
        /// </summary>
        /// <param name="nChoice">The number of the choice.</param>
        /// <param name="choice">The name of the Choice.</param>
        private static void DisplayChoice(int nChoice, string choice)
        {
            choice = ti.ToTitleCase(choice.ToLower());

            nChoice++;

            string text = $"{nChoice} - {choice}";
            Console.WriteLine(text);
        }

        private static void AskForExit()
        {
            ConsoleKey key;
            if (!DataOperations.SavedConfig)
            {
                do
                {
                    Console.WriteLine("There is unsaved configuration.\n");

                    if (DataOperations.DistinctTypes)
                    {
                        DisplayFormats();
                    }

                    if (DataOperations.DistinctValues)
                    {
                        Console.WriteLine("\n-------------------------\n");
                        DisplayExtensionFormats();
                    }

                    Console.WriteLine("Save the configuration before closing? (Y/N)");
                    key = Console.ReadKey().Key;
                    Console.Clear();

                    if (key == ConsoleKey.Y)
                    {
                        DataOperations.SaveConfig();
                    }
                }
                while (key != ConsoleKey.Y && key != ConsoleKey.N);
            }
        }
    }
}
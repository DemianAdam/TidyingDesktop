// <copyright file="ConfigMenuActions.cs" company="DnamSolutions">
// Copyright (c) DnamSolutions. All rights reserved.
// </copyright>

namespace TidyingDesktop.UI.Menus.ConfigMenu
{
    using TidyingDesktop.StaticClasses;
    using TidyingDesktop.UI.Menus;

    /// <summary>
    /// Represents the actions of the Configuration Menu.
    /// </summary>
    internal static class ConfigMenuActions
    {
        private static readonly string[] ActionNames = new string[]
        {
            "Formats",
            "Save configuration",
            "Include Folders",
            "Change folder to Order",
            "Set Order folder to Desktop",
        }
        .Select(s => s.ToUpper()).ToArray();

        /// <summary>
        /// Gets the actions of the Configuration Menu.
        /// </summary>
        public static List<SingleAction> Actions
        {
            get
            {
                List<SingleAction> actions = new List<SingleAction>
                {
                    new SingleAction(ActionNames[1], DisplaySaveConfig),
                    new SingleAction(ActionNames[2], DisplayIncludeFolders),
                    new SingleAction(ActionNames[3], ChangeFolder),
                    new SingleAction(ActionNames[4], SetFolderToDesktop),
                };
                return actions;
            }
        }

        private static void DisplaySaveConfig()
        {
            bool distinctTypes = DataOperations.DistinctTypes;
            bool distinctValues = DataOperations.DistinctValues;
            ConsoleKey consoleKey = ConsoleKey.N;

            if (distinctTypes || distinctValues)
            {
                Console.WriteLine("Unsaved configuration:\n");

                if (distinctTypes)
                {
                    if (DataOperations.DistinctTypes)
                    {
                        MenuUI.DisplayFormats();
                    }
                }

                if (distinctValues)
                {
                    Console.WriteLine("\n-------------------------\n");

                    if (DataOperations.DistinctValues)
                    {
                        MenuUI.DisplayExtensionFormats();
                    }
                }

                Console.WriteLine("Save the configuration? (Y/N)");
                consoleKey = Console.ReadKey().Key;
                Console.Clear();

                if (consoleKey == ConsoleKey.Y)
                {
                    try
                    {
                        DataOperations.SaveConfig();
                    }
                    catch (Exception ex)
                    {
                        consoleKey = ConsoleKey.E;
                        Console.WriteLine("Error on saving configuration. Error: " + ex.Message);
                    }
                }

                if (consoleKey == ConsoleKey.Y)
                {
                    Console.WriteLine("Configuration saved..");
                }
                else
                {
                    Console.WriteLine("Configuration not saved.");
                }
            }
            else
            {
                Console.WriteLine("Configuration already Saved.");
            }
        }

        private static void DisplayIncludeFolders()
        {
            DataOperations.Configuration.IncludeFolders = !DataOperations.Configuration.IncludeFolders;
            Console.WriteLine($"Include folders is set to: {DataOperations.Configuration.IncludeFolders}");
        }

        private static void ChangeFolder()
        {
            string? path;
            int choice = 0;

            do
            {
                Console.WriteLine("Type the folder to order (Type nothing to exit):");
                Console.WriteLine($"1. Origin: {DataOperations.Configuration.OriginDirectoryPath}");
                Console.WriteLine($"2. Destination: {DataOperations.Configuration.DestinationDirectoryPath}");
                choice = MenuUI.UserChoice(2);
            }
            while (choice < 0);

            Console.Write("Type the new path of the folder(Type nothing to exit): ");
            path = Console.ReadLine();

            if (path == string.Empty)
            {
                return;
            }

            try
            {
                if (path is not null)
                {
                    switch (choice)
                    {
                        case 1:
                            DataOperations.Configuration.OriginDirectoryPath = path;
                            break;
                        case 2:
                            DataOperations.Configuration.DestinationDirectoryPath = path;
                            break;
                    }

                    Console.WriteLine($"Folder changed successfuly to {path}.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error when changing the folder. " + ex.Message);
            }
        }

        private static void SetFolderToDesktop()
        {
            try
            {
                DataOperations.Configuration.OriginDirectoryPath = DataOperations.Configuration.DesktopPath;
                Console.WriteLine($"Folder changed successfuly to {DataOperations.Configuration.OriginDirectoryPath}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error when changing the folder. " + ex.Message);
            }
        }
    }
}
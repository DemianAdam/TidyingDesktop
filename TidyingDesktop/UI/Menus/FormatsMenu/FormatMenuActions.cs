// <copyright file="FormatMenuActions.cs" company="DnamSolutions">
// Copyright (c) DnamSolutions. All rights reserved.
// </copyright>

namespace TidyingDesktop.UI.Menus.FormatsMenu
{
    using TidyingDesktop.StaticClasses;

    /// <summary>
    /// Represents the actions of the Formats Menu.
    /// </summary>
    internal class FormatMenuActions
    {
        private static readonly string[] ActionNames = new FormatMenuConfig().MenuChoices.Select(s => s = s.ToUpper()).ToArray();

        /// <summary>
        /// Gets the actions of the Formats Menu.
        /// </summary>
        public static List<SingleAction> Actions
        {
            get
            {
                List<SingleAction> actions = new List<SingleAction>
                {
                    new SingleAction(ActionNames[0], DisplayListOfExtensionFormat),
                    new SingleAction(ActionNames[1], DisplayAddNewExtensionFormat),
                    new SingleAction(ActionNames[2], DisplayAddFormat),
                    new SingleAction(ActionNames[3], DisplayModifyExtensionFormat),
                };

                return actions;
            }
        }

        private static void DisplayListOfExtensionFormat()
        {
            int count = 1;
            foreach (var item in DataOperations.ExtensionTypePair)
            {
                Console.WriteLine($"[{count}] {item}");
                count++;
            }
        }

        private static void DisplayAddNewExtensionFormat()
        {
            string? extension;
            string? format;

            format = ChooseFormat();

            if (format == "EXIT")
            {
                Console.WriteLine("Exited");
                return;
            }

            extension = ChooseExtension();

            if (extension == "EXIT")
            {
                Console.WriteLine("Exited");
                return;
            }

            try
            {
                DataOperations.AddFormat(extension, format);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected exception" + ex.Message);
            }
        }

        private static void DisplayAddFormat()
        {
            string? format;
            bool added = false;

            Console.WriteLine("List of formats: ");
            foreach (var item in DataOperations.Types)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine("New format (Type nothing to exit): ");
            format = Console.ReadLine();

            if (string.IsNullOrEmpty(format))
            {
                Console.WriteLine("Exited");
                return;
            }

            added = DataOperations.AddFormatType(format);

            if (added)
            {
                Console.WriteLine("Format added successfully.");
            }
            else
            {
                Console.WriteLine("Format already exist.");
            }
        }

        private static void DisplayModifyExtensionFormat()
        {
            int nFormat;
            string? extension;
            string? format;
            bool result = false;

            do
            {
                Console.WriteLine("Choose the number of the element to modify (type nothing to exit): ");
                DisplayListOfExtensionFormat();
                nFormat = MenuUI.UserChoice(DataOperations.ExtensionTypePair.Count());

                if (nFormat == 0 || nFormat == -1)
                {
                    Console.WriteLine("Invalid input.");
                }

                if (nFormat == -2)
                {
                    Console.WriteLine("Exited");
                    return;
                }
            }
            while (nFormat == 0 || nFormat == -1);

            nFormat--;

            format = ChooseFormat();

            if (format == "EXIT")
            {
                Console.WriteLine("Exited");
                return;
            }

            extension = ChooseExtension();

            if (extension == "EXIT")
            {
                Console.WriteLine("Exited");
                return;
            }

            try
            {
                result = DataOperations.ModifyDictionaryElement(nFormat, extension, format);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error when modifying element. " + ex.Message);
            }

            if (result)
            {
                Console.WriteLine("Element modified successfully.");
            }
            else
            {
                Console.WriteLine("Element not modified.");
            }
        }

        private static string ChooseFormat()
        {
            int nFormat;
            int count;
            string format;
            do
            {
                count = 0;
                Console.WriteLine("Choose a number of format from the list (type nothing to exit): ");

                foreach (var item in DataOperations.Types)
                {
                    Console.WriteLine($"[{count + 1}] {item}");
                    count++;
                }

                nFormat = MenuUI.UserChoice(count);

                if (nFormat == 0 || nFormat == -1)
                {
                    Console.WriteLine("Invalid input.");
                }

                if (nFormat == -2)
                {
                    return "EXIT";
                }
            }
            while (nFormat == 0 || nFormat == -1);

            format = DataOperations.Types.ToArray()[nFormat - 1];

            return format;
        }

        private static string ChooseExtension()
        {
            string? extension;
            Console.WriteLine("New extension (type nothing to exit): ");
            extension = Console.ReadLine();

            if (string.IsNullOrEmpty(extension))
            {
                return "EXIT";
            }

            return extension;
        }
    }
}

// <copyright file="Program.cs" company="DnamSolutions">
// Copyright (c) DnamSolutions. All rights reserved.
// </copyright>

namespace TidyingDesktop
{
    using System.Configuration;
    using TidyingDesktop.StaticClasses;
    using TidyingDesktop.UI.Menus;
    using TidyingDesktop.UI.Menus.MainMenu;

    /// <summary>
    /// Start point of the application.
    /// </summary>
    internal class Program
    {
        private static void Main(string[] args)
        {
            MenuUI.DisplayMenu(new MainMenuConfig());
        }
    }
}
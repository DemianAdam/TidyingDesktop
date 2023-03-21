// <copyright file="MainMenuConfig.cs" company="DnamSolutions">
// Copyright (c) DnamSolutions. All rights reserved.
// </copyright>

namespace TidyingDesktop.UI.Menus.MainMenu
{
    using TidyingDesktop.UI.Menus.ConfigMenu;

    /// <summary>
    /// Represents the configuration of the Main Menu.
    /// </summary>
    internal class MainMenuConfig : IMenuConfig
    {
        /// <inheritdoc/>
        public string Name => "Main Menu".ToUpper();

        /// <inheritdoc/>
        public string[] MenuChoices => new string[] { "Order", "Restore BackUp", "Configuration" };

        /// <inheritdoc/>
        public List<SingleAction>? MenuActions => MainMenuActions.Actions;

        /// <inheritdoc/>
        public IMenuConfig? ParentMenu => null;

        /// <inheritdoc/>
        public List<IMenuConfig>? ChildMenus
        {
            get => new List<IMenuConfig>
            {
                new ConfigMenuConfig(),
            };
        }
    }
}

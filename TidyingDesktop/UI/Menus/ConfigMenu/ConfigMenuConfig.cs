// <copyright file="ConfigMenuConfig.cs" company="DnamSolutions">
// Copyright (c) DnamSolutions. All rights reserved.
// </copyright>

namespace TidyingDesktop.UI.Menus.ConfigMenu
{
    using TidyingDesktop.UI.Menus.FormatsMenu;
    using TidyingDesktop.UI.Menus.MainMenu;

    /// <summary>
    /// Represents the configuration of the Configuration Menu.
    /// </summary>
    internal class ConfigMenuConfig : IMenuConfig
    {
        /// <inheritdoc/>
        public string Name => "Configuration".ToUpper();

        /// <inheritdoc/>
        public string[] MenuChoices => new string[] { "Formats", "Save configuration", "Include Folders", "Change folder to Order", "Set Order folder to Desktop" };

        /// <inheritdoc/>
        public List<SingleAction>? MenuActions => ConfigMenuActions.Actions;

        /// <inheritdoc/>
        public IMenuConfig? ParentMenu => new MainMenuConfig();

        /// <inheritdoc/>
        public List<IMenuConfig>? ChildMenus => new List<IMenuConfig>
            {
                new FormatMenuConfig(),
            };
    }
}
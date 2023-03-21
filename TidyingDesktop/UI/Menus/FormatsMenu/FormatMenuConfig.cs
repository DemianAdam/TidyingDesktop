// <copyright file="FormatMenuConfig.cs" company="DnamSolutions">
// Copyright (c) DnamSolutions. All rights reserved.
// </copyright>

namespace TidyingDesktop.UI.Menus.FormatsMenu
{
    using TidyingDesktop.UI.Menus.ConfigMenu;

    /// <summary>
    /// Represents the configuration of the Formats Menu.
    /// </summary>
    internal class FormatMenuConfig : IMenuConfig
    {
        /// <inheritdoc/>
        public string Name => "Formats".ToUpper();

        /// <inheritdoc/>
        public string[] MenuChoices => new string[] { "List of Extension-Formats", "Add new Extension-Format", "Add new Format", "Modify Extension-Format" };

        /// <inheritdoc/>
        public List<SingleAction>? MenuActions => FormatMenuActions.Actions;

        /// <inheritdoc/>
        public IMenuConfig? ParentMenu => new ConfigMenuConfig();

        /// <inheritdoc/>
        public List<IMenuConfig>? ChildMenus => null;
    }
}

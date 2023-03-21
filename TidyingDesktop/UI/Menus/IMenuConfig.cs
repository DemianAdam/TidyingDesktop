// <copyright file="IMenuConfig.cs" company="DnamSolutions">
// Copyright (c) DnamSolutions. All rights reserved.
// </copyright>

namespace TidyingDesktop.UI.Menus
{
    /// <summary>
    /// Represents the configuration of a menu.
    /// </summary>
    internal interface IMenuConfig
    {
        /// <summary>
        /// Gets the name of the menu.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the choices of the menu.
        /// </summary>
        public string[] MenuChoices { get; }

        /// <summary>
        /// Gets the actions of the menu.
        /// </summary>
        public List<SingleAction>? MenuActions { get; }

        /// <summary>
        /// Gets the configuration of the parent menu.
        /// </summary>
        public IMenuConfig? ParentMenu { get; }

        /// <summary>
        /// Gets the list of the child menus.
        /// </summary>
        public List<IMenuConfig>? ChildMenus { get; }

        /// <summary>
        /// Executes an action of the <see cref="IMenuConfig.MenuActions"/> with the name associated.
        /// </summary>
        /// <param name="actionName">The <see cref="SingleAction.ActionName"/> of the action.</param>
        public void Action(string actionName)
        {
            SingleAction? action = this.MenuActions?.Find(a => a.ActionName == actionName.ToUpper());
            action?.Action.Invoke();
            MenuUI.PauseMessage();
        }
    }
}

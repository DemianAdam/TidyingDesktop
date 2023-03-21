// <copyright file="SingleAction.cs" company="DnamSolutions">
// Copyright (c) DnamSolutions. All rights reserved.
// </copyright>

namespace TidyingDesktop.UI.Menus
{
    /// <summary>
    /// Represents an Action with a name.
    /// </summary>
    internal class SingleAction
    {
        private readonly string actionName;

        private readonly Action action;

        private readonly string[]? parameters;

        /// <summary>
        /// Initializes a new instance of the <see cref="SingleAction"/> class.
        /// </summary>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="action">The <see cref="System.Action"/>.</param>
        /// <param name="parameters">the parameters of the action.</param>
        public SingleAction(string actionName, Action action, string[]? parameters = null)
        {
            this.actionName = actionName.ToUpper();
            this.action = action;
            this.parameters = parameters;
        }

        /// <summary>
        /// Gets the action name.
        /// </summary>
        public string ActionName { get => this.actionName; }

        /// <summary>
        /// Gets the <see cref="System.Action"/>.
        /// </summary>
        public Action Action { get => this.action; }
    }
}

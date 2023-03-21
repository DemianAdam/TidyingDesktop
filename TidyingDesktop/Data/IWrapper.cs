// <copyright file="IWrapper.cs" company="DnamSolutions">
// Copyright (c) DnamSolutions. All rights reserved.
// </copyright>

namespace TidyingDesktop.Data
{
    /// <summary>
    /// Represents a Wrapper for the <see cref="FileInfo"/> or the <see cref="DirectoryInfo"/> class.
    /// </summary>
    public interface IWrapper
    {
        /// <summary>
        /// Gets the creation time of the element.
        /// </summary>
        DateTime CreationTime { get; }

        /// <summary>
        /// Gets the Full name of the element.
        /// </summary>
        string FullName { get; }

        /// <summary>
        /// Gets the name of the element.
        /// </summary>
        string Name { get; }
    }
}
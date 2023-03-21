// <copyright file="DirectoryInfoWrapper.cs" company="DnamSolutions">
// Copyright (c) DnamSolutions. All rights reserved.
// </copyright>

namespace TidyingDesktop.Data
{
    /// <summary>
    /// Represents a Wrapper for the <see cref="DirectoryInfo"/> class.
    /// </summary>
    public class DirectoryInfoWrapper : IWrapper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryInfoWrapper"/> class.
        /// </summary>
        /// <param name="creationTime">The <see cref="DirectoryInfo"/>.CreationTime property.</param>
        /// <param name="fullName">The <see cref="DirectoryInfo"/>.FullName property.</param>
        /// <param name="name">The <see cref="DirectoryInfo"/>.Name property.</param>
        public DirectoryInfoWrapper(DateTime creationTime, string fullName, string name)
        {
            this.CreationTime = creationTime;
            this.FullName = fullName;
            this.Name = name;
        }

        /// <inheritdoc/>
        public DateTime CreationTime { get; }

        /// <inheritdoc/>
        public string FullName { get; }

        /// <inheritdoc/>
        public string Name { get; }
    }
}

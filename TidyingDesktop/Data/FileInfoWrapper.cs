// <copyright file="FileInfoWrapper.cs" company="DnamSolutions">
// Copyright (c) DnamSolutions. All rights reserved.
// </copyright>

namespace TidyingDesktop.Data
{
    /// <summary>
    /// Represents a Wrapper for the <see cref="FileInfo"/> class.
    /// </summary>
    public class FileInfoWrapper : IWrapper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileInfoWrapper"/> class, wich acts as a wrapper for the <see cref="FileInfo"/>.CreationTime, <see cref="FileInfo"/>.FullName and the <see cref="FileInfo"/>.Extension Properties.
        /// </summary>
        /// <param name="creationTime">Represents the <see cref="FileInfo"/>.CreationTime Property.</param>
        /// <param name="fullName">Represents the <see cref="FileInfo"/>.FullName Property.</param>
        /// /// <param name="name">Represents the <see cref="FileInfo"/>.Name Property.</param>
        /// <param name="extension">Represents the <see cref="FileInfo"/>.Extension Property.</param>
        public FileInfoWrapper(DateTime creationTime, string fullName, string name, string extension)
        {
            this.CreationTime = creationTime;
            this.FullName = fullName;
            this.Name = name;
            this.Extension = extension;
        }

        /// <summary>
        /// Gets the Creation Time of the file.
        /// </summary>
        public DateTime CreationTime { get; }

        /// <summary>
        /// Gets the Full Name of the file.
        /// </summary>
        public string FullName { get; }

        /// <summary>
        /// Gets the Name of the file.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the Extension of the file.
        /// </summary>
        public string Extension { get; }
    }
}
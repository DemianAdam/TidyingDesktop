// <copyright file="Extensions.cs" company="DnamSolutions">
// Copyright (c) DnamSolutions. All rights reserved.
// </copyright>

namespace TidyingDesktop
{
    using TidyingDesktop.Data;

    /// <summary>
    /// Represent the extensions methods of the application.
    /// </summary>
    internal static class Extensions
    {
        /// <inheritdoc cref="Enumerable.ToList{TSource}(IEnumerable{TSource})"/>
        public static List<FileInfoWrapper> ToFileInfoWrapperList(this List<FileInfo> fileInfos)
        {
            return new List<FileInfoWrapper>(fileInfos.Select(a => a.ToFileInfoWrapper()));
        }

        /// <summary>
        /// Converts a <see cref="FileInfo"/> to a <see cref="FileInfoWrapper"/>.
        /// </summary>
        /// <param name="fileInfo">The <see cref="FileInfo"/> to convert.</param>
        /// <returns>An instance of a <see cref="FileInfoWrapper"/>.</returns>
        public static FileInfoWrapper ToFileInfoWrapper(this FileInfo fileInfo)
        {
            return new FileInfoWrapper(fileInfo.CreationTime, fileInfo.FullName, fileInfo.Name, fileInfo.Extension);
        }

        /// <inheritdoc cref="Enumerable.ToList{TSource}(IEnumerable{TSource})"/>
        public static List<DirectoryInfoWrapper> ToDirectoryInfoWrapperList(this List<DirectoryInfo> directoryInfo)
        {
            return new List<DirectoryInfoWrapper>(directoryInfo.Select(a => a.ToDirectoryInfoWrapper()));
        }

        /// <summary>
        /// Converts a <see cref="DirectoryInfo"/> to a <see cref="DirectoryInfoWrapper"/>.
        /// </summary>
        /// <param name="directoryInfo">The <see cref="DirectoryInfo"/> to convert.</param>
        /// <returns>An instance of a <see cref="DirectoryInfoWrapper"/>.</returns>
        public static DirectoryInfoWrapper ToDirectoryInfoWrapper(this DirectoryInfo directoryInfo)
        {
            return new DirectoryInfoWrapper(directoryInfo.CreationTime, directoryInfo.FullName, directoryInfo.Name);
        }
    }
}

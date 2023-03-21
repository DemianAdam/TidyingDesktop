// <copyright file="BackUpFile.cs" company="DnamSolutions">
// Copyright (c) DnamSolutions. All rights reserved.
// </copyright>

namespace TidyingDesktop
{
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using TidyingDesktop.Data;
    using TidyingDesktop.StaticClasses;

    /// <summary>
    /// Represents a BackUp File.
    /// </summary>
    public class BackUpFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BackUpFile"/> class.
        /// </summary>
        /// <param name="files">The <see cref="IEnumerable{T}"/> of files.</param>
        /// <param name="directories">The <see cref="IEnumerable{T}"/> of directories.</param>
        public BackUpFile(IEnumerable<FileInfoWrapper> files, IEnumerable<DirectoryInfoWrapper> directories)
        {
            this.Directories = directories.ToList();
            this.Files = files.ToList();
            this.OriginDirectory = DataOperations.Configuration.OriginDirectoryPath;
            this.DestinationDirectory = DataOperations.Configuration.DestinationDirectoryPath;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BackUpFile"/> class.
        /// </summary>
        public BackUpFile()
        {
            this.Files = new List<FileInfoWrapper>();
            this.Directories = new List<DirectoryInfoWrapper>();
            this.OriginDirectory = DataOperations.Configuration.OriginDirectoryPath;
            this.DestinationDirectory = DataOperations.Configuration.DestinationDirectoryPath;
        }

        /// <summary>
        /// Gets the list of files of the backup.
        /// </summary>
        [JsonInclude]
        public List<FileInfoWrapper> Files { get; private set; }

        /// <summary>
        /// Gets the list of files of the backup.
        /// </summary>
        [JsonInclude]
        public List<DirectoryInfoWrapper> Directories { get; private set; }

        /// <summary>
        /// Gets the origin directory of the files.
        /// </summary>
        [JsonInclude]
        public string OriginDirectory { get; private set; }

        /// <summary>
        /// Gets the destination directory of the files.
        /// </summary>
        [JsonInclude]
        public string DestinationDirectory { get; private set; }

        /// <summary>
        /// Deserialize a <see cref="BackUpFile"/> instance from the path of <see cref="DirectoryConfiguration.BackUpFileName"/>.
        /// </summary>
        /// <returns>A <see cref="BackUpFile"/>.</returns>
        /// <exception cref="JsonException">Deserialization returns a null <see cref="BackUpFile"/>.</exception>
        public static BackUpFile Restore()
        {
            if (!File.Exists(DataOperations.Configuration.BackUpFileName))
            {
                throw new FileNotFoundException();
            }

            string json = File.ReadAllText(DataOperations.Configuration.BackUpFileName);
            BackUpFile? backup = JsonSerializer.Deserialize<BackUpFile>(json);

            if (backup is null)
            {
                throw new JsonException();
            }

            return backup;
        }

        /// <summary>
        /// Serialize this instance of <see cref="BackUpFile"/> in the path of <see cref="DirectoryConfiguration.BackUpFileName"/>.
        /// </summary>
        public void BackUp()
        {
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.WriteIndented = true;
            options.IncludeFields = true;
            string json = JsonSerializer.Serialize(this, options);
            File.WriteAllText(DataOperations.Configuration.BackUpFileName, json);
        }
    }
}
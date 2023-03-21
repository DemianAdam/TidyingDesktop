// <copyright file="DirectoryConfiguration.cs" company="DnamSolutions">
// Copyright (c) DnamSolutions. All rights reserved.
// </copyright>

namespace TidyingDesktop.Data
{
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text.Json;

    /// <summary>
    /// Represents a configuration of the directories.
    /// </summary>
    internal class DirectoryConfiguration
    {
        private const string FOLDERNAME = "Tidying Desktop";

        private const string RECENTFILENAME = "f01b4d95cf55d32a.automaticDestinations-ms";

        private string originDirectoryPath;

        private string destinationDirectoryPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryConfiguration"/> class.
        /// </summary>
        /// <param name="os">The current operating system.</param>
        public DirectoryConfiguration(OSPlatform os)
        {
            this.DesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            this.AppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            this.ConfigPath = Path.Combine(this.AppDataPath, FOLDERNAME, "Config");
            this.BackUpPath = Path.Combine(this.AppDataPath, FOLDERNAME, "Backup");

            this.CreateDirectories(this.ConfigPath, this.BackUpPath);

            this.ConfigExtensionFormatsFileName = Path.Combine(this.ConfigPath, "ExtensionsFormats.json");
            this.ConfigFormatsFileName = Path.Combine(this.ConfigPath, "Formats.json");

            this.CreateFiles();

            this.BackUpFileName = Path.Combine(this.BackUpPath, "Backup.json");

            if (os == OSPlatform.Windows)
            {
                try
                {
                    this.RecentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Recent);
                    string file = Directory.GetDirectories(this.RecentsPath).First(e => Path.GetFileName(e) == "AutomaticDestinations");
                    string fileName = Path.Combine(file, RECENTFILENAME);
                    if (File.Exists(fileName))
                    {
                        this.AutomaticDestinationsPath = fileName;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }

            this.originDirectoryPath = this.DesktopPath;

            this.destinationDirectoryPath = this.SetDestinationPath(this.originDirectoryPath);

            this.IncludeFolders = true;
        }

        /// <summary>
        /// Gets the desktop folder path.
        /// </summary>
        public string DesktopPath { get; }

        /// <summary>
        /// Gets the appdata folder path.
        /// </summary>
        public string AppDataPath { get; }

        /// <summary>
        /// Gets the recents folder path.
        /// </summary>
        public string? RecentsPath { get; }

        /// <summary>
        /// Gets the configuration folder path.
        /// </summary>
        public string ConfigPath { get; }

        /// <summary>
        /// Gets the backup foldder path.
        /// </summary>
        public string BackUpPath { get; }

        /// <summary>
        /// Gets or sets the path of the directory that will be order.
        /// </summary>
        public string OriginDirectoryPath
        {
            get
            {
                return this.originDirectoryPath;
            }

            set
            {
                this.ValidatePath(value);
                this.destinationDirectoryPath = this.SetDestinationPath(value);
                this.originDirectoryPath = value;
            }
        }

        /// <summary>
        /// Gets or sets the path of the directory that will be where the ordering happen.
        /// </summary>
        public string DestinationDirectoryPath
        {
            get
            {
                return this.destinationDirectoryPath;
            }

            set
            {
                this.ValidatePath(value);
                this.destinationDirectoryPath = value;
            }
        }

        /// <summary>
        /// Gets the path of the folder that contains the Recent Files information.
        /// </summary>
        public string? AutomaticDestinationsPath { get; }

        /// <summary>
        /// Gets the name of the backup file.
        /// </summary>
        public string BackUpFileName { get; }

        /// <summary>
        /// Gets the name of the config file that will contain the extensions and formats.
        /// </summary>
        public string ConfigExtensionFormatsFileName { get; }

        /// <summary>
        /// Gets the name of the config file that will contain the formats.
        /// </summary>
        public string ConfigFormatsFileName { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the folders should be included in the ordering.
        /// </summary>
        public bool IncludeFolders { get; set; }

        private void CreateFiles()
        {
            FormatDictionary temp = new FormatDictionary();
            FormatDictionaryWrapper wrapper = new FormatDictionaryWrapper(temp);
            string jsonDictionary = JsonSerializer.Serialize(wrapper);

            if (!File.Exists(this.ConfigExtensionFormatsFileName))
            {
                File.Create(this.ConfigExtensionFormatsFileName).Close();
                File.WriteAllText(this.ConfigExtensionFormatsFileName, jsonDictionary);
            }

            string jsonFormats = JsonSerializer.Serialize(temp.Types);

            if (!File.Exists(this.ConfigFormatsFileName))
            {
                File.Create(this.ConfigFormatsFileName).Close();
                File.WriteAllText(this.ConfigFormatsFileName, jsonFormats);
            }
        }

        private void CreateDirectories(string path, params string[] paths)
        {
            List<string> dirs = paths.ToList();
            dirs.Add(path);
            foreach (string dir in dirs)
            {
                this.CreateDirectoryIfDoesntExist(dir);
            }
        }

        private void CreateDirectoryIfDoesntExist(string fullpath)
        {
            this.ValidatePath(fullpath);
            if (!Directory.Exists(fullpath))
            {
                Directory.CreateDirectory(fullpath).Refresh();
            }
        }

        private string SetDestinationPath(string value)
        {
            this.ValidatePath(value);
            string folderName = "Ordered " + new DirectoryInfo(value).Name;
            return Path.Combine(value, folderName);
        }

        private void ValidatePath(string path)
        {
            string? validPath = Path.GetPathRoot(path);

            if (validPath is null)
            {
                throw new ArgumentException();
            }
            else if (validPath == string.Empty)
            {
                throw new DirectoryNotFoundException();
            }
        }
    }
}

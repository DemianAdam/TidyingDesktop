// <copyright file="DataOperations.cs" company="DnamSolutions">
// Copyright (c) DnamSolutions. All rights reserved.
// </copyright>

namespace TidyingDesktop.StaticClasses
{
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text.Json;
    using TidyingDesktop.Data;
    using TidyingDesktop.UI.Menus.MainMenu;

    /// <summary>
    /// Represents the operations performed on the data.
    /// </summary>
    internal static class DataOperations
    {
        /// <inheritdoc cref="FormatDictionary"/>
        private static FormatDictionary formatDictionary;

        private static FormatDictionary sessionData;

        private static BackUpFile? backUpFile;

        private static DirectoryConfiguration dirConfiguration;

        private static OSPlatform oSPlatform;

        static DataOperations()
        {
            oSPlatform = OSPlatform.Windows;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                oSPlatform = OSPlatform.OSX;
            }

            try
            {
                dirConfiguration = new DirectoryConfiguration(oSPlatform);
            }
            catch (Exception)
            {
                throw;
            }

            Message += DataOperations_Message;
            formatDictionary = new FormatDictionary();
            sessionData = new FormatDictionary();
            formatDictionary.CollectionChanged += FormatDictionary_CollectionChanged;
            formatDictionary.SetChanged += FormatDictionary_SetChanged;
            LoadConfig();
        }

        /// <summary>
        /// Represents a message when an operation happen.
        /// </summary>
        public static event NotifyAction? Message;

        /// <summary>
        /// Gets an <see cref="IEnumerable{T}"/> of files in the current directory specified in <see cref="DirectoryConfiguration.OriginDirectoryPath"/>.
        /// </summary>
        public static IEnumerable<FileInfoWrapper> CurrentDirectoryFiles
        {
            get { return Files.ToFileInfoWrapperList(); }
        }

        /// <summary>
        /// Gets an <see cref="IEnumerable{T}"/> of directories in the current directory specified in <see cref="DirectoryConfiguration.OriginDirectoryPath"/>.
        /// </summary>
        public static IEnumerable<DirectoryInfoWrapper> CurrentDirectoryDirectories
        {
            get { return Directories.ToDirectoryInfoWrapperList(); }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="ExtensionTypePair"/> property is saved.
        /// </summary>
        public static bool SavedConfig { get; private set; }

        /// <summary>
        /// Gets an <see cref="IEnumerable{T}"/> of the format types in the <see cref="Dictionary{TKey, TValue}"/>.
        /// </summary>
        public static IEnumerable<string> TypeExtension
        {
            get { return formatDictionary.Values; }
        }

        /// <summary>
        /// Gets an <see cref="IEnumerable{T}"/> of the format extensions in the <see cref="Dictionary{TKey, TValue}"/>.
        /// </summary>
        public static IEnumerable<string> FileExtensions
        {
            get { return formatDictionary.Keys; }
        }

        /// <summary>
        /// Gets an <see cref="IEnumerable{T}"/> of the format types in the <see cref="SortedSet{T}"/>.
        /// </summary>
        public static IEnumerable<string> Types
        {
            get { return formatDictionary.Types; }
        }

        /// <summary>
        /// Gets an instance of <see cref="BackUpFile"/> restored from <see cref="DirectoryConfiguration.BackUpFileName"/>.
        /// </summary>
        public static BackUpFile RestoredBackup
        {
            get
            {
                BackUpFile backUp;
                try
                {
                    backUp = BackUpFile.Restore();
                }
                catch (Exception)
                {
                    throw;
                }

                return backUp;
            }
        }

        /// <summary>
        /// Gets an <see cref="IEnumerable{T}"/> of the format types in the <see cref="SortedSet{T}"/> of this Session.
        /// </summary>
        public static IEnumerable<string> SessionTypes
        {
            get { return sessionData.Types; }
        }

        /// <summary>
        /// Gets an <see cref="IEnumerable{T}"/> of the <see cref="FormatDictionary.ExtensionFormatPair"/> with the data of this Session.
        /// </summary>
        public static IEnumerable<KeyValuePair<string, string>> SessionData { get => sessionData.ExtensionFormatPair; }

        /// <summary>
        /// Gets an <see cref="IEnumerable{T}"/> of the <see cref="FormatDictionary.ExtensionFormatPair"/>.
        /// </summary>
        public static IEnumerable<KeyValuePair<string, string>> ExtensionTypePair
        {
            get => formatDictionary.ExtensionFormatPair;
        }

        /// <summary>
        /// Gets the files of the <see cref="DirectoryConfiguration.OriginDirectoryPath"/>.
        /// </summary>
        public static List<FileInfo> Files
        {
            get
            {
                List<FileInfo> files = new List<FileInfo>();

                foreach (var item in Directory.GetFiles(dirConfiguration.OriginDirectoryPath))
                {
                    files.Add(new FileInfo(item));
                }

                return files;
            }
        }

        /// <summary>
        /// Gets the files of the <see cref="DirectoryConfiguration.OriginDirectoryPath"/>.
        /// </summary>
        public static List<DirectoryInfo> Directories
        {
            get
            {
                List<DirectoryInfo> directories = new List<DirectoryInfo>();

                foreach (var item in Directory.GetDirectories(dirConfiguration.OriginDirectoryPath))
                {
                    directories.Add(new DirectoryInfo(item));
                }

                return directories;
            }
        }

        /// <summary>
        /// Gets a value indicating whether if the session types and the loaded types are different.
        /// </summary>
        public static bool DistinctTypes
        {
            get
            {
                bool distinct = false;
                int countSessionTypes = SessionTypes.Count();
                int countAllTypes = Types.Count();

                if (countAllTypes == countSessionTypes)
                {
                    var types = Types.ToArray();
                    var sessionTypes = SessionTypes.ToArray();

                    for (int i = 0; i < countAllTypes; i++)
                    {
                        if (types[i] != sessionTypes[i])
                        {
                            distinct = true;
                            break;
                        }
                    }
                }
                else
                {
                    distinct = true;
                }

                return distinct;
            }
        }

        /// <summary>
        /// Gets a value indicating whether if the session values and the loaded values are different.
        /// </summary>
        public static bool DistinctValues
        {
            get
            {
                bool distinct = false;
                int countSessionData = SessionData.Count();
                int countAll = ExtensionTypePair.Count();

                if (countSessionData == countAll)
                {
                    var sessionDataArray = SessionData.ToArray();
                    var allArray = ExtensionTypePair.ToArray();
                    for (int i = 0; i < countAll; i++)
                    {
                        if ((sessionDataArray[i].Key != allArray[i].Key) || (sessionDataArray[i].Value != allArray[i].Value))
                        {
                            distinct = true;
                            break;
                        }
                    }
                }
                else
                {
                    distinct = true;
                }

                return distinct;
            }
        }

        /// <summary>
        /// Gets the <see cref="DirectoryConfiguration"/> of the application.
        /// </summary>
        internal static DirectoryConfiguration Configuration { get => dirConfiguration; }

        /// <summary>
        /// Order the files of the <see cref="CurrentDirectoryFiles"/> to the <see cref="DirectoryConfiguration.DestinationDirectoryPath"/>.
        /// </summary>
        public static void OrderFiles()
        {
            Message?.Invoke("Ordering files...");
            int sleep = 25;
            DirectoryInfo destination = new DirectoryInfo(dirConfiguration.DestinationDirectoryPath);
            List<FileInfoWrapper> filesWrappers = new List<FileInfoWrapper>();
            List<DirectoryInfoWrapper> directoryWrappers = new List<DirectoryInfoWrapper>();
            if (!Directory.Exists(destination.FullName))
            {
                Directory.CreateDirectory(destination.FullName);
                Message?.Invoke($"Creating directory: {destination.FullName}");
            }

            TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
            foreach (var formatType in Types)
            {
                string folderName = ti.ToTitleCase(formatType.ToLower());
                destination.CreateSubdirectory(folderName);

                Message?.Invoke($"\nCreating Sub-Directory: {folderName}\n");

                var keys = ExtensionTypePair.Where(a => a.Value == formatType);
                var allfiles = from files in CurrentDirectoryFiles
                               join key in keys
                               on files.Extension.ToUpper() equals key.Key.ToUpper()
                               select files;

                foreach (var file in allfiles)
                {
                    string path = Path.Combine(destination.FullName, folderName, Path.GetFileName(file.FullName));
                    File.Move(file.FullName, path);
                    Message?.Invoke($"\t Moving {file.Name} to {folderName}");
                    FileInfo fileInfo = new FileInfo(path);
                    filesWrappers.Add(fileInfo.ToFileInfoWrapper());
                    Thread.Sleep(sleep);
                }
            }

            if (dirConfiguration.IncludeFolders)
            {
                Message?.Invoke("\nOrdering Folders.");
                foreach (var item in Directories)
                {
                    if (item.FullName == dirConfiguration.DestinationDirectoryPath)
                    {
                        continue;
                    }

                    string path = Path.Combine(destination.FullName, item.Name);
                    Directory.Move(item.FullName, path);
                    Message?.Invoke($"\tMoving {item.Name} folder to {destination.Name}");
                    DirectoryInfo directoryInfo = new DirectoryInfo(path);
                    directoryWrappers.Add(directoryInfo.ToDirectoryInfoWrapper());
                    Thread.Sleep(sleep);
                }
            }

            backUpFile = new BackUpFile(filesWrappers, directoryWrappers);

            OrderOriginDirectory();
            RefreshQuickAccessFile();
            Thread.Sleep(2000);
            Message?.Invoke($"{dirConfiguration.OriginDirectoryPath} Ordered.");
        }

        /// <summary>
        /// Serializes and saves the <see cref="FormatDictionary"/> to the directory specified in <see cref="DirectoryConfiguration.ConfigExtensionFormatsFileName"/>.
        /// </summary>
        public static void SaveConfig()
        {
            FormatDictionaryWrapper wrapper = new FormatDictionaryWrapper(formatDictionary);
            string jsonDictionary = JsonSerializer.Serialize(wrapper);

            if (!File.Exists(dirConfiguration.ConfigExtensionFormatsFileName))
            {
                File.Create(dirConfiguration.ConfigExtensionFormatsFileName).Close();
            }

            File.WriteAllText(dirConfiguration.ConfigExtensionFormatsFileName, jsonDictionary);

            string jsonFormats = JsonSerializer.Serialize(formatDictionary.Types);

            if (!File.Exists(dirConfiguration.ConfigFormatsFileName))
            {
                File.Create(dirConfiguration.ConfigFormatsFileName).Close();
            }

            File.WriteAllText(dirConfiguration.ConfigFormatsFileName, jsonFormats);
            SavedConfig = true;
        }

        /// <summary>
        /// Deserializes and load the <see cref="FormatDictionary"/> from <see cref="DirectoryConfiguration.ConfigExtensionFormatsFileName"/> path.
        /// </summary>
        /// <exception cref="JsonException">If the deserialization return null.</exception>
        public static void LoadConfig()
        {
            if (File.Exists(dirConfiguration.ConfigFormatsFileName))
            {
                string json = File.ReadAllText(dirConfiguration.ConfigFormatsFileName);
                SortedSet<string>? temp = JsonSerializer.Deserialize<SortedSet<string>>(json);

                if (temp is null)
                {
                    throw new JsonException();
                }

                foreach (var item in temp)
                {
                    formatDictionary.AddType(item);
                    sessionData.AddType(item);
                }
            }

            if (File.Exists(dirConfiguration.ConfigExtensionFormatsFileName))
            {
                string json = File.ReadAllText(dirConfiguration.ConfigExtensionFormatsFileName);
                FormatDictionaryWrapper? temp = JsonSerializer.Deserialize<FormatDictionaryWrapper>(json);

                if (temp is null)
                {
                    throw new JsonException();
                }

                foreach (var item in temp.KeyValuePairs)
                {
                    formatDictionary.Add(item);
                    sessionData.Add(item);
                }
            }
            else
            {
                SaveConfig();
            }

            SavedConfig = true;
        }

        /// <summary>
        /// Adds a format to <see cref="FormatDictionary"/>.
        /// </summary>
        /// <param name="extension">The extension of the format to add.</param>
        /// <param name="type">The type of the element to add. Has to be contained in <see cref="FormatDictionary.types"/>.</param>
        /// <inheritdoc cref="FormatDictionary.Add(string, string)" path="/exception"/>
        public static void AddFormat(string extension, string type)
        {
            formatDictionary.Add(extension, type);
        }

        /// <summary>
        /// <inheritdoc cref="FormatDictionary.AddType(string)"/>
        /// </summary>
        /// <param name="type"> <inheritdoc cref="FormatDictionary.AddType(string)" path="/param[@name='item']"/></param>
        /// <returns><see langword="true"/> if the element is added to the <see cref="FormatDictionary.Values"/> object; <see langword="false"/> if the element is already present.</returns>
        public static bool AddFormatType(string type)
        {
            return formatDictionary.AddType(type);
        }

        /// <summary>
        /// Modifys an element of the <see cref="FormatDictionary"/>.
        /// </summary>
        /// <param name="index">The index to modify.</param>
        /// <param name="extension">The new extension.</param>
        /// <param name="format">The new format.</param>
        /// <returns><see langword="true"/> if the element if modified successfully. Otherwise <see langword="false"/>.</returns>
        public static bool ModifyDictionaryElement(int index, string extension, string format)
        {
            bool result = false;
            var item = formatDictionary[index];

            result = formatDictionary.UpdateElement(item.Key, extension, format);

            return result;
        }

        /// <summary>
        /// Makes a backup of the Ordered directories.
        /// </summary>
        public static void MakeBackUp()
        {
            try
            {
                backUpFile?.BackUp();
                Message?.Invoke("Making Backup.");
            }
            catch (Exception ex)
            {
                Message?.Invoke(ex.Message);
            }
        }

        /// <summary>
        /// Restores the backup saved in the <see cref="DirectoryConfiguration.BackUpFileName"/>.
        /// </summary>
        public static void RestoreBackup()
        {
            int sleep = 25;
            BackUpFile backUp = RestoredBackup;

            Message?.Invoke("Restoring Files.");

            foreach (var item in backUp.Files)
            {
                try
                {
                    string path = Path.Combine(backUp.OriginDirectory, item.Name);
                    File.Move(item.FullName, path);
                    Message?.Invoke($"Moving {item.Name} to {backUp.OriginDirectory}");
                }
                catch (FileNotFoundException ex)
                {
                    Message?.Invoke(ex.Message);
                }

                Thread.Sleep(sleep);
            }

            Message?.Invoke("Restoring Directories.");

            foreach (var item in backUp.Directories)
            {
                try
                {
                    string path = Path.Combine(backUp.OriginDirectory, item.Name);
                    Directory.Move(item.FullName, path);
                    Message?.Invoke($"Moving {item.Name} to {backUp.OriginDirectory}");
                }
                catch (DirectoryNotFoundException ex)
                {
                    Message?.Invoke(ex.Message);
                }

                Thread.Sleep(sleep);
            }
        }

        private static void OrderOriginDirectory()
        {
            string tempDirPath = Path.Combine(dirConfiguration.OriginDirectoryPath, "TEMP");

            if (!Directory.Exists(tempDirPath))
            {
                Directory.CreateDirectory(tempDirPath);
            }

            DirectoryInfo directoryInfo = new DirectoryInfo(tempDirPath);

            foreach (var item in Directories)
            {
                if ((item.FullName == tempDirPath) || item.FullName == dirConfiguration.DestinationDirectoryPath)
                {
                    continue;
                }

                Directory.Move(item.FullName, Path.Combine(tempDirPath, item.Name));
            }

            Parallel.ForEach(CurrentDirectoryFiles, (currentFile) =>
            {
                File.Move(currentFile.FullName, Path.Combine(tempDirPath, Path.GetFileName(currentFile.FullName)));
            });

            Thread.Sleep(2000);

            List<DirectoryInfo> directoryInfos = new List<DirectoryInfo>();

            foreach (var item in Directory.EnumerateDirectories(tempDirPath))
            {
                directoryInfos.Add(new DirectoryInfo(item));
            }

            foreach (var item in directoryInfos)
            {
                Directory.Move(item.FullName, Path.Combine(dirConfiguration.OriginDirectoryPath, item.Name));
            }

            List<FileInfo> files = new List<FileInfo>();

            foreach (var item in directoryInfo.EnumerateFiles())
            {
                File.Move(item.FullName, Path.Combine(dirConfiguration.OriginDirectoryPath, item.Name));
            }

            if (Directory.Exists(tempDirPath))
            {
                Directory.Delete(tempDirPath);
            }
        }

        private static void RefreshQuickAccessFile()
        {
            if (File.Exists(dirConfiguration.AutomaticDestinationsPath))
            {
                Message?.Invoke("Refreshing Quick Access File.");
                File.Delete(dirConfiguration.AutomaticDestinationsPath);
                File.Create(dirConfiguration.AutomaticDestinationsPath);
            }
        }

        private static void FormatDictionary_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            SavedConfig = false;
        }

        private static void FormatDictionary_SetChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            SavedConfig = false;
        }

        private static void DataOperations_Message(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}

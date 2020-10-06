using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Xml.Linq;

namespace ThpsSaveManager
{
    public static class SaveUtilities
    {
        #region Paths

        private static string AppDataPath
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            }
        }

        private static string GameSavePath
        {
            get
            {
                return $"{AppDataPath}/VicariousVisions/THPS/Saved/SaveGames/";
            }
        }

        private static string LocalSaveFolder
        {
            get
            {
                return "./Saves/";
            }
        }

        private static string LocalConfigFolder
        {
            get
            {
                return "./Config/";
            }
        }

        private static string LocalBackupFolder
        {
            get
            {
                return "./Backup/";
            }
        }

        private static string LocalSavePath(string saveName)
        {
            return $"{LocalSaveFolder}/{saveName}.zip";
        }

        private static string LocalConfigPath(string saveName)
        {
            return $"{LocalConfigFolder}/{saveName}.xml";
        }

        private static string LocalBackupPath(string loadedFileName)
        {
            var dateString = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            return $"{LocalBackupFolder}/Save before loading {loadedFileName} at {dateString}.zip";
        }

        #endregion

        #region Save Config

        public static void LoadConfig(SaveListElementViewModel save)
        {
            try
            {
                var config = XElement.Load(LocalConfigPath(save.Name));
                save.FromXml(config);
            }
            catch (FileNotFoundException)
            {
                // We don't have a config, so make one
                SaveConfig(save);
            }
        }

        public static void SaveConfig(SaveListElementViewModel save)
        {
            var config = save.ToXml();
            config.Save(LocalConfigPath(save.Name));
        }

        #endregion

        #region Basic Functions

        public static void NewSave(string saveName)
        {
            File.Create(LocalSavePath(saveName));
            Events.StatusText($"Created blank game save {saveName}.");
        }

        public static void Initialize()
        {
            Events.StatusText("Initializing game folders...");
            Directory.CreateDirectory(LocalSaveFolder);
            Directory.CreateDirectory(LocalConfigFolder);
            Directory.CreateDirectory(LocalBackupFolder);
            Directory.CreateDirectory(GameSavePath);
        }

        public static void Save(string saveName)
        {
            File.Delete(LocalSavePath(saveName));
            ZipFile.CreateFromDirectory(GameSavePath, LocalSavePath(saveName));
            Events.StatusText($"Saved into save file {saveName}.");
        }

        private const int NumBackups = 5;

        public static void CleanBackups()
        {
            var directory = new DirectoryInfo(LocalBackupFolder);
            var files = directory.GetFiles();

            // Sort all files by last edit date
            var sortedFiles = from file in files
                              orderby file.LastWriteTime
                              select file;

            // Delete the first (N - numBackups) so we now have numBackups files
            for (int i = 0; i < files.Length - NumBackups; i++)
            {
                File.Delete(files[i].FullName);
            }
        }

        public static void Backup(string loadedFileName)
        {
            ZipFile.CreateFromDirectory(GameSavePath, LocalBackupPath(loadedFileName));
            CleanBackups();
        }

        public static void Load(string saveName, bool disableSaving)
        {
            Backup(saveName);

            // Re-enable saving so we can delete the directory
            ToggleSaving(true);

            Directory.Delete(GameSavePath, true);
            Directory.CreateDirectory(GameSavePath);

            var file = new FileInfo(LocalSavePath(saveName));
            if (file.Length > 0)
            {
                ZipFile.ExtractToDirectory(LocalSavePath(saveName), GameSavePath);
            }

            // Since we re-made the directory we have to set the files as save/readonly again
            ToggleSaving(!disableSaving);

            Events.StatusText($"Loaded save file {saveName}.");
        }

        public static void Delete(string saveName)
        {
            File.Delete(LocalSavePath(saveName));
            File.Delete(LocalConfigPath(saveName));
            Events.StatusText($"Deleted save file {saveName}.");
        }

        public static void Clone(string saveName, string cloneName)
        {
            File.Copy(LocalSavePath(saveName), LocalSavePath(cloneName));
            File.Copy(LocalConfigPath(saveName), LocalConfigPath(cloneName));
            Events.StatusText($"Cloned save file {saveName} into {cloneName}.");
        }

        public static void Rename(string oldName, string newName)
        {
            File.Move(LocalSavePath(oldName), LocalSavePath(newName));
            File.Move(LocalConfigPath(oldName), LocalConfigPath(newName));
            Events.StatusText($"Renamed save file {oldName} to {newName}.");
        }

        public static IEnumerable<string> LocalSaveFiles()
        {
            return from savePath in Directory.GetFiles(LocalSaveFolder)
                   select ExtractSaveName(savePath);
        }

        private static string ExtractSaveName(string savePath)
        {
            var beginning = savePath.LastIndexOf("/") + 1;
            var end = savePath.LastIndexOf(".");

            return savePath.Substring(beginning, end - beginning);
        }

        #endregion

        #region Options

        public static void ToggleSaving(bool enable)
        {
            var saveDirectory = new DirectoryInfo(GameSavePath);
            if (enable)
            {
                MakeWritable(saveDirectory);
                Events.StatusText("Made game folder writable.");
            }
            else
            {
                MakeReadOnly(saveDirectory);
                Events.StatusText("Made game folder read-only.");
            }
        }

        public static void MakeReadOnly(DirectoryInfo directory)
        {
            directory.Attributes |= FileAttributes.ReadOnly;

            foreach (FileInfo file in directory.GetFiles())
            {
                file.Attributes |= FileAttributes.ReadOnly;
            }

            foreach (DirectoryInfo subDirectory in directory.GetDirectories())
            {
                MakeReadOnly(subDirectory);
            }
        }

        public static void MakeWritable(DirectoryInfo directory)
        {
            directory.Attributes &= ~FileAttributes.ReadOnly;

            foreach (FileInfo file in directory.GetFiles())
            {
                file.Attributes &= ~FileAttributes.ReadOnly;
            }

            foreach (DirectoryInfo subDirectory in directory.GetDirectories())
            {
                MakeWritable(subDirectory);
            }
        }

        #endregion
    }
}

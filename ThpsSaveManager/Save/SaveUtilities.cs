using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

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

        private static string LocalSavePath(string saveName)
        {
            return $"./{LocalSaveFolder}/{saveName}.zip";
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
            Directory.CreateDirectory(GameSavePath);
        }

        public static void Save(string saveName)
        {
            File.Delete(LocalSavePath(saveName));
            ZipFile.CreateFromDirectory(GameSavePath, LocalSavePath(saveName));
            Events.StatusText($"Saved into save file {saveName}.");
        }

        public static void Load(string saveName)
        {
            var savingDisabled = Convert.ToBoolean(Settings.Get("DisableSaving") ?? "false");

            // Re-enable saving so we can delete the directory
            if (savingDisabled)
                ToggleSaving(true);

            Directory.Delete(GameSavePath, true);
            Directory.CreateDirectory(GameSavePath);

            var file = new FileInfo(LocalSavePath(saveName));
            if (file.Length > 0)
            {
                ZipFile.ExtractToDirectory(LocalSavePath(saveName), GameSavePath);
            }

            // Since we re-made the directory we have to set the files as save/readonly again
            ToggleSaving(!savingDisabled);

            Events.StatusText($"Loaded save file {saveName}.");
        }

        public static void Delete(string saveName)
        {
            File.Delete(LocalSavePath(saveName));
            Events.StatusText($"Deleted save file {saveName}.");
        }

        public static void Clone(string saveName, string cloneName)
        {
            File.Copy(LocalSavePath(saveName), LocalSavePath(cloneName));
            Events.StatusText($"Cloned save file {saveName} into {cloneName}.");
        }

        public static void Rename(string oldName, string newName)
        {
            File.Move(LocalSavePath(oldName), LocalSavePath(newName));
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

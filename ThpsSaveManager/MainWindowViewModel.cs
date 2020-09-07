using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThpsSaveManager
{
    public class MainWindowViewModel : ViewModelBase
    {
        private bool _initializing = true;

        private string _statusText = "";
        public string StatusText { 
            get
            {
                return _statusText;
            }
            set
            {
                SetProperty(ref _statusText, value);
            }
        }

        private bool _disableSaving = false;
        public bool DisableSaving
        {
            get
            {
                return _disableSaving;
            }
            set
            {
                if (!_initializing)
                    SaveUtilities.ToggleSaving(!value);

                SetProperty(ref _disableSaving, value);
                //Settings.AddUpdate("DisableSaving", value.ToString());
            }
        }

        private bool _disableIntros = false;
        public bool DisableIntros
        {
            get
            {
                return _disableIntros;
            }
            set
            {
                if (!_initializing)
                    ToggleIntros(!value);

                SetProperty(ref _disableIntros, value);
                Settings.AddUpdate("DisableIntros", value.ToString());
            }
        }

        public bool ValidGameFolder
        {
            get
            {
                return GameFolder.EndsWith("TonyHawksProSkater");
            }
        }

        private string _gameFolder = "";
        public string GameFolder
        {
            get
            {
                return _gameFolder;
            }
            set
            {
                SetProperty(ref _gameFolder, value);
                OnPropertyChanged(nameof(ValidGameFolder)); 

                Settings.AddUpdate("GameFolder", value);
            }
        }

        public string VideosFolder
        {
            get
            {
                return GameFolder + "/Base/Content/Movies/";
            }
        }

        public MainWindowViewModel()
        {
            _initializing = true;

            GameFolder = Settings.Get("GameFolder") ?? "";
            DisableIntros = Convert.ToBoolean(Settings.Get("DisableIntros") ?? "false");
            DisableSaving = Convert.ToBoolean(Settings.Get("DisableSaving") ?? "false");

            _initializing = false;

            Events.SetStatusText += (text) => StatusText = text;
        }

        private void ToggleIntros(bool enable)
        {
            if (enable)
            {
                // Trim the '.bak' off the end of each file
                foreach (var videoFile in Directory.GetFiles(VideosFolder))
                {
                    var newName = videoFile.Substring(0, videoFile.Length - 4);
                    File.Move(videoFile, newName);
                }

                StatusText = "Enabled intro videos.";
            }
            else
            {
                // Add '.bak' to the end of each file
                foreach (var videoFile in Directory.GetFiles(VideosFolder))
                {
                    File.Move(videoFile, videoFile + ".bak");
                }

                StatusText = "Disabled intro videos.";
            }
        }
    }
}

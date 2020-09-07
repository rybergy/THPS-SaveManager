using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ThpsSaveManager
{
    public delegate void SaveEvent(SaveEventArgs e);

    public class SaveListElementViewModel : ViewModelBase
    {
        public event SaveEvent Cloned;
        public event SaveEvent Deleted;

        private bool _initializing = true;

        private string _name = "";
        public string Name 
        { 
            get
            {
                return _name;
            }
            set
            {
                SetProperty(ref _name, value);
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
                SetProperty(ref _disableSaving, value);

                if (!_initializing)
                    SaveConfig();
            }
        }

        public SaveListElementViewModel(string name)
        {
            _initializing = true;

            Name = name;
            LoadConfig();

            _initializing = false;
        }

        public void Save()
        {
            SaveUtilities.Save(Name);
        }

        public void Load()
        {
            SaveUtilities.Load(Name, DisableSaving);
        }

        public void Rename(string saveName)
        {
            SaveUtilities.Rename(Name, saveName);
            Name = saveName;
        }

        public void Clone()
        {
            Cloned?.Invoke(new SaveEventArgs(this));
        }

        public void Delete()
        {
            if (ConfirmationDialog.Make("Delete File", "Are you sure you want to delete this save file?"))
                Deleted?.Invoke(new SaveEventArgs(this));
        }

        public XElement ToXml()
        {
            return new XElement("Save",
                new XElement("DisableSaving", DisableSaving));
        }

        public void FromXml(XElement x)
        {
            DisableSaving = Convert.ToBoolean(x.Element("DisableSaving").Value);
        }

        public void SaveConfig()
        {
            SaveUtilities.SaveConfig(this);
        }

        public void LoadConfig()
        {
            SaveUtilities.LoadConfig(this);
        }
    }
}

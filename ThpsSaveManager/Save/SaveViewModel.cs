using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThpsSaveManager
{
    public delegate void SaveEvent(SaveEventArgs e);

    public class SaveViewModel : ViewModelBase
    {
        public event SaveEvent Cloned;
        public event SaveEvent Deleted;

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

        public SaveViewModel(string name)
        {
            Name = name;
        }

        public void Save()
        {
            SaveUtilities.Save(Name);
        }

        public void Load()
        {
            SaveUtilities.Load(Name);
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
    }
}

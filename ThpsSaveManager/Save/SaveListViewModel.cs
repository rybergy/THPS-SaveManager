using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThpsSaveManager
{
    public class SaveListViewModel : ViewModelBase
    {
        public ObservableCollection<SaveListElementViewModel> Saves { get; set; } = new ObservableCollection<SaveListElementViewModel>();

        public SaveListViewModel()
        {
            foreach (var saveName in SaveUtilities.LocalSaveFiles())
            {
                Console.WriteLine($"Creating save with name {saveName}");
                CreateNew(saveName);
            }
        }

        public SaveListElementViewModel CreateNew(string saveName)
        {
            var save = new SaveListElementViewModel(saveName);
            save.Cloned += CloneSave;
            save.Deleted += DeleteSave;

            Saves.Add(save);

            return save;
        }

        private string CloneName(string saveName)
        {
            var cloneName = saveName + " (Copy)";

            while (Saves.Any(save => save.Name == cloneName))
                cloneName += " (Copy)";

            return cloneName;
        }

        public void CloneSave(SaveEventArgs e)
        {
            var newName = CloneName(e.Save.Name);
            SaveUtilities.Clone(e.Save.Name, newName);
            CreateNew(newName);
        }

        public void DeleteSave(SaveEventArgs e)
        {
            e.Save.Cloned -= CloneSave;
            e.Save.Deleted -= DeleteSave;

            SaveUtilities.Delete(e.Save.Name);

            Saves.Remove(e.Save);
        }
    }
}

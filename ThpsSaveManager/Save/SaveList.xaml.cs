using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ThpsSaveManager
{
    /// <summary>
    /// Interaction logic for SaveList.xaml
    /// </summary>
    public partial class SaveList : UserControl
    {
        private SaveListViewModel ViewModel = new SaveListViewModel();

        public SaveList()
        {
            InitializeComponent();

            DataContext = ViewModel;
        }

        public void CreateNew()
        {
            string saveName = InputDialog.Make("Save Name", "Please enter a name for this save.", regexString: @"^[\w,\s-]+$");
            if (saveName != null)
            {
                ViewModel.CreateNew(saveName);
                SaveUtilities.NewSave(saveName);
            }   
        }
    }
}

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
    /// Interaction logic for SaveListElement.xaml
    /// </summary>
    public partial class SaveListElement : UserControl
    {
        private SaveListElementViewModel ViewModel
        {
            get
            {
                return (SaveListElementViewModel)DataContext;
            }
        }

        public SaveListElement()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Save();
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Load();
        }

        private void btnRename_Click(object sender, RoutedEventArgs e)
        {
            string saveName = InputDialog.Make("Save Name", "Please enter a name for this save.", regexString: @"^[\w,\s-]+$");
            if (saveName != null)
            {
                ViewModel.Rename(saveName);
            }
        }

        private void btnClone_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Clone();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Delete();
        }
    }
}

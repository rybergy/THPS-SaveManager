using AdonisUI.Controls;
using System.Windows;
using Ookii.Dialogs.Wpf;

namespace ThpsSaveManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : AdonisWindow
    {
        public MainWindowViewModel ViewModel = new MainWindowViewModel();

        public MainWindow()
        {
            InitializeComponent();

            DataContext = ViewModel;
        }

        private void NewSave_Click(object sender, RoutedEventArgs e)
        {
            PART_SaveList.CreateNew();
        }

        private void GameFolder_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new VistaFolderBrowserDialog();
            if (dialog.ShowDialog() ?? false)
            {
                ViewModel.GameFolder = dialog.SelectedPath;
            }
        }
    }
}

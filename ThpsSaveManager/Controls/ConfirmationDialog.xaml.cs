using AdonisUI.Controls;
using System.Windows;

namespace ThpsSaveManager
{
    /// <summary>
    /// Interaction logic for ConfirmationDialog.xaml
    /// </summary>
    public partial class ConfirmationDialog : AdonisWindow
    {
        private ConfirmationDialog(string title, string description)
        {
            InitializeComponent();

            Title = title;
            txtDescription.Text = description;
        }

        public static bool Make(string title, string description)
        {
            var dialog = new ConfirmationDialog(title, description);
            return dialog.ShowDialog() ?? false;
        }

        private void Yes_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void No_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}

using AdonisUI.Controls;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ThpsSaveManager
{
    /// <summary>
    /// Interaction logic for InputDialog.xaml
    /// </summary>
    public partial class InputDialog : AdonisWindow
    {
        private Regex _regex;
        private string _name;

        private InputDialog(string name, string description, string regexString)
        {
            InitializeComponent();

            _name = name;
            this.Title = $"Enter a {name}";
            txtDescription.Text = description;

            if (regexString != null) 
            {
                _regex = new Regex(regexString);
            }
            
            CheckValidity();

            txtInput.Focus();
            Keyboard.Focus(txtInput);
        }

        public static string Make(string name, string description, string regexString = null)
        {
            var input = new InputDialog(name, description, regexString);
            if (input.ShowDialog() ?? false)
            {
                return input.txtInput.Text;
            }
            else
            {
                return null;
            }
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void txtInput_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    DialogResult = false;
                    break;
                case Key.Enter:
                    DialogResult = true;
                    break;
                default:
                    CheckValidity();
                    break;
            }
        }

        private void CheckValidity()
        {
            var match = _regex?.Match(txtInput.Text);
            if (match?.Success ?? false)
            {
                txtError.Visibility = Visibility.Collapsed;
                txtError.Text = "";

                btnOk.IsEnabled = true;
            }
            else
            {
                txtError.Visibility = Visibility.Visible;
                txtError.Text = $"Please enter a valid {_name.ToLowerInvariant()}";

                btnOk.IsEnabled = false;
            }
        }
    }
}

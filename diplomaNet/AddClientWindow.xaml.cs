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
using System.Windows.Shapes;

namespace diplomaNet
{
    /// <summary>
    /// Логика взаимодействия для AddClientWindow.xaml
    /// </summary>
    public partial class AddClientWindow : Window
    {
        public AddClientWindow()
        {
            InitializeComponent();
        }

        public AddClientWindow(string lastNameTextBox, string firstNameTextBox, string patronymicTextBox, string addressTextBox, string phoneTextBox)
        {
            InitializeComponent();
            this.lastNameTextBox.Text = lastNameTextBox;
            this.firstNameTextBox.Text = firstNameTextBox;
            this.patronymicTextBox.Text = patronymicTextBox;
            this.addressTextBox.Text = addressTextBox;
            this.phoneTextBox.Text = phoneTextBox;
            okBtn.Content = "Применить";
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}

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

namespace diplomaNet
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void phoneList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //modelTextBox.Text = ((Phone)phoneList.SelectedItems[0]).Title;
            //manufacturerTextBox.Text = ((Phone)phoneList.SelectedItems[0]).Company;
            //priceTextBox.Text = ((Phone)phoneList.SelectedItems[0]).Price.ToString();
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            //((Phone)phoneList.SelectedItems[0]).Title = modelTextBox.Text;
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            Phone phone = new Phone();
            phone.Title = modelTextBox.Text;
            phone.Company = manufacturerTextBox.Text;
            phone.Price = Int32.Parse(priceTextBox.Text);
        }
    }

}

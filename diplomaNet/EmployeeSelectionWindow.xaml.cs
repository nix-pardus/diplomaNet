//using ServiceCenterApi.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для EmployeeSelectionWindow.xaml
    /// </summary>
    public partial class EmployeeSelectionWindow : Window
    {
        public ObservableCollection<Employee> employees { get; set; }
        public EmployeeSelectionWindow()
        {
            InitializeComponent();
        }
        public EmployeeSelectionWindow(ObservableCollection<Employee> employees)
        {
            InitializeComponent();
            
            this.employees = employees;
        }

        private void okBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}

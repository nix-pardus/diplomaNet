using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using diplomaNet;
using Newtonsoft.Json;
//using ServiceCenterApi.Models;

namespace diplomaNet
{
    public partial class MainWindow : Window
    {
        // Строка системных сообщений
        public string LabelString { get; set; }
        // Список заказов
        public ObservableCollection<Order> Orders { get; set; }
        // Список клиентов
        public ObservableCollection<Client> Clients { get; set; }
        // Список сотрудников
        public ObservableCollection<Employee> Employees { get; set; }
        

        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
            LabelString = WebRequestHelper.TestConnection.Test();
            if (LabelString == "ok")
            {
                Orders = new ObservableCollection<Order>();
                Clients = new ObservableCollection<Client>();
                Employees = new ObservableCollection<Employee>();
                Orders = GetOrders();
                Clients = GetClients();
                Employees = GetEmployees();
                LabelString = "";
            }
        }

        private ObservableCollection<Order> GetOrders()
        {
            return WebRequestHelper.Get<Order>.Send("current", null);
        }

        private Order GetOrder(int id)
        {
            return WebRequestHelper.Get<Order>.Send("getOrder", id);
        }

        private ObservableCollection<Client> GetClients()
        {
            return WebRequestHelper.Get<Client>.Send("clients", null);
        }
        private ObservableCollection<Employee> GetEmployees()
        {
            return WebRequestHelper.Get<Employee>.Send("employees", null);
        }

        private ObservableCollection<Work> GetWorks(List<Work> works)
        {
            string dataString = "";
            foreach (Work work in works)
            {
                dataString += $"{work.Name}={work.Price}&";
            }
            dataString = dataString.Remove(dataString.Length - 1);
            string query = Uri.EscapeDataString(dataString);
            return WebRequestHelper.Get<Work>.Send("works", query.ToString());
        }

        // Отображает все заказы выбранного клиента
        private void clientsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count < 1)
                return;
            Client client = (Client)e.AddedItems[0];
            customerOrdersListView.ItemsSource = Orders.Where(o => o.Client.Id == client.Id).ToList();
        }

        // Добавляет нового клиента в БД
        private void addClientButton_Click(object sender, RoutedEventArgs e)
        {
            mWindow.Opacity = 0.5;
            AddClientWindow addClientWindow = new AddClientWindow();
            addClientWindow.Owner = this;
            if (addClientWindow.ShowDialog() == true)
            {
                mWindow.Opacity = 1;

                Client client = new Client();
                client.LastName = addClientWindow.lastNameTextBox.Text;
                client.FirstName = addClientWindow.firstNameTextBox.Text;
                client.Address = addClientWindow.addressTextBox.Text;
                client.Patronymic = addClientWindow.patronymicTextBox.Text;
                client.Phone = addClientWindow.phoneTextBox.Text;

                if (WebRequestHelper.Post.Send(client, "addClient"))
                    Clients.Add(GetClients().Last());
            }
            else
                mWindow.Opacity = 1;
        }

        // Удаляет клиента
        private void deleteClientButton_Click(object sender, RoutedEventArgs e)
        {
            if (clientsListView.SelectedItems.Count == 0)
                return;
            var selectedClients = clientsListView.SelectedItems;
            MessageBoxResult result = MessageBox.Show($"Вы собираетесь удалить {selectedClients.Count} клиента(ов)", "Предкпреждение", MessageBoxButton.OKCancel, MessageBoxImage.Asterisk);
            if (result == MessageBoxResult.Cancel)
                return;
            int[] ids = new int[selectedClients.Count];
            int i = 0;
            List<Client> clientsTemp = new List<Client>();
            foreach (Client client in selectedClients)
            {
                clientsTemp.Add(client);
                ids[i++] = client.Id;
            }
            if (WebRequestHelper.Delete.Send(ids, "deleteClient"))
                foreach (Client client in clientsTemp)
                {
                    Clients.Remove(client);
                }
        }

        // Запускает окна редактирования клиента
        private void editClientButton_Click(object sender, RoutedEventArgs e)
        {
            if (clientsListView.SelectedItems.Count == 0)
                return;
            mWindow.Opacity = 0.5;
            Client client = clientsListView.SelectedItem as Client;
            int id = client.Id;
            if (client == null)
                return;
            AddClientWindow editClientWindow = new AddClientWindow(
                client.LastName,
                client.FirstName,
                client.Patronymic,
                client.Address,
                client.Phone);
            editClientWindow.Owner = this;
            if (editClientWindow.ShowDialog() == true)
            {
                client = null;
                client = new Client();
                client.Id = id;
                client.LastName = editClientWindow.lastNameTextBox.Text;
                client.FirstName = editClientWindow.firstNameTextBox.Text;
                client.Patronymic = editClientWindow.patronymicTextBox.Text;
                client.Address = editClientWindow.addressTextBox.Text;
                client.Phone = editClientWindow.phoneTextBox.Text;
                if (WebRequestHelper.Put.Send(client, "editClient"))
                {
                    int index = Clients.IndexOf(Clients.FirstOrDefault(x => x.Id == id));
                    Clients[index] = client;
                }
            }
            mWindow.Opacity = 1;
        }

        // Добавляет заказ
        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            mWindow.Opacity = 0.5;
            AddOrderWindow addOrderWindow = new AddOrderWindow(Clients);
            addOrderWindow.Owner = this;
            if (addOrderWindow.ShowDialog() == true)
            {
                mWindow.Opacity = 1;
                Order order = new Order();
                order.Date = DateTime.Now;
                List<Work> worksTemp = new List<Work>();
                foreach (Work el in addOrderWindow.workList)
                {
                    worksTemp.Add(el);
                }
                order.Client = addOrderWindow.client;
                order.WorkList = GetWorks(worksTemp).ToList();
                order.Employees = new List<Employee>();

                if (WebRequestHelper.Post.Send(order, "addOrder"))
                    Orders.Add(GetOrders().Last());
            }
            else
            {
                mWindow.Opacity = 1;
            }
        }

        // Отображает все заказы выбранного сотрудника
        private void employeesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count < 1)
                return;
            Employee employee = (Employee)e.AddedItems[0];
            employeeOrdersListView.ItemsSource = Orders
                .Where(o => o.Employees.Where(x => x.Id == employee.Id).Any())
                .ToList();
        }

        // Добавляет нового сотрудника
        private void addEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            mWindow.Opacity = 0.5;
            AddEmployeeWindow addEmployeeWindow = new AddEmployeeWindow();
            addEmployeeWindow.Owner = this;
            if (addEmployeeWindow.ShowDialog() == true)
            {
                Employee employee = new Employee();
                employee.LastName = addEmployeeWindow.lastNameTextBox.Text;
                employee.FirstName = addEmployeeWindow.firstNameTextBox.Text;
                employee.Patronymic = addEmployeeWindow.patronymicTextBox.Text;
                employee.Birthdate = DateTime.Parse(addEmployeeWindow.birthdateTextBox.Text);
                employee.Address = addEmployeeWindow.addressTextBox.Text;
                employee.Phone = addEmployeeWindow.phoneTextBox.Text;

                if (WebRequestHelper.Post.Send(employee, "addEmployee"))
                    Employees.Add(GetEmployees().Last());
            }
            mWindow.Opacity = 1;
        }

        // Запускает окно редактирования сотрудника
        private void editEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            if (employeesListView.SelectedItems.Count == 0)
                return;
            mWindow.Opacity = 0.5;
            Employee employee = employeesListView.SelectedItem as Employee;
            int id = employee.Id;
            AddEmployeeWindow addEmployeeWindow = new AddEmployeeWindow(
                employee.LastName,
                employee.FirstName,
                employee.Patronymic,
                employee.Birthdate.ToShortDateString(),
                employee.Address,
                employee.Phone);
            addEmployeeWindow.Owner = this;
            if (addEmployeeWindow.ShowDialog() == true)
            {
                employee = null;
                employee = new Employee();
                employee.Id = id;
                employee.LastName = addEmployeeWindow.lastNameTextBox.Text;
                employee.FirstName = addEmployeeWindow.firstNameTextBox.Text;
                employee.Patronymic = addEmployeeWindow.patronymicTextBox.Text;
                employee.Birthdate = DateTime.Parse(addEmployeeWindow.birthdateTextBox.Text);
                employee.Address = addEmployeeWindow.addressTextBox.Text;
                employee.Phone = addEmployeeWindow.phoneTextBox.Text;
                if (WebRequestHelper.Put.Send(employee, "editEmployee"))
                {
                    int index = Employees.IndexOf(Employees.FirstOrDefault(x => x.Id == id));
                    Employees[index] = employee;
                }
            }
            mWindow.Opacity = 1;
        }

        // Удаляет сотрудника
        private void deleteEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            if (employeesListView.SelectedItems.Count == 0)
                return;
            var selectedEmployees = employeesListView.SelectedItems;
            MessageBoxResult result = MessageBox.Show($"Вы собираетесь удалить {selectedEmployees.Count} работника(ов)! Продолжить?", "Предупреждение", MessageBoxButton.OKCancel, MessageBoxImage.Asterisk);
            if (result == MessageBoxResult.Cancel)
                return;
            int[] ids = new int[selectedEmployees.Count];
            int i = 0;
            List<Employee> employeesTemp = new List<Employee>();
            foreach (Employee employee in selectedEmployees)
            {
                employeesTemp.Add(employee);
                ids[i++] = employee.Id;
            }
            if (WebRequestHelper.Delete.Send(ids, "deleteEmployees"))
            {
                foreach (Employee employee in employeesTemp)
                    Employees.Remove(employee);
            }
        }

        // Назначает сотрудника(ов) на заказ
        private void selectEmployeeBtn_Click(object sender, RoutedEventArgs e)
        {
            mWindow.Opacity = 0.5;
            EmployeeSelectionWindow employeeSelectionWindow = new EmployeeSelectionWindow(Employees);
            employeeSelectionWindow.Owner = this;
            if (employeeSelectionWindow.ShowDialog() == true)
            {
                Order order = (Order)orderListView.SelectedItem;
                List<Employee> employees = employeeSelectionWindow.employeeSelectionListView.SelectedItems.Cast<Employee>().ToList();
                Dictionary<int, List<int>> data = new Dictionary<int, List<int>>();
                List<int> ids = new List<int>();
                foreach(Employee employee in employees)
                {
                    ids.Add(employee.Id);
                }
                data.Add(order.Id, ids);
                if (WebRequestHelper.Put.Send(data, "editEmployeesInOrder"))
                {
                    int index = Orders.IndexOf(Orders.FirstOrDefault(x => x.Id == order.Id));
                    var o = GetOrder(order.Id);
                    Orders[index] = o;
                    orderListView.SelectedIndex = index;
                }
            }
            mWindow.Opacity = 1;
        }

        // Удаляет выбранный заказ
        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if(orderListView.SelectedItem == null)
                return;
            mWindow.Opacity = 0.5;
            MessageBoxResult messageBoxResult = MessageBox.Show("Вы собираетесь удалить заказ! Продолжить?", "Предупреждение", MessageBoxButton.OKCancel, MessageBoxImage.Asterisk);
            if(messageBoxResult == MessageBoxResult.OK)
            {
                int[] ids = new int[1];
                ids[0] = ((Order)orderListView.SelectedItem).Id;
                if (WebRequestHelper.Delete.Send(ids, "deleteOrder"))
                {
                    Orders.RemoveAt(orderListView.SelectedIndex);
                }
            }
            mWindow.Opacity = 1;
        }

        // Запускает окно редактирования заказа
        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            AddOrderWindow addOrderWindow = new AddOrderWindow(Clients, (Order)orderListView.SelectedItem);
            addOrderWindow.Owner = this;
            if(addOrderWindow.ShowDialog() == true)
            {
                Order order = (Order)orderListView.SelectedItem;
                order.Client = addOrderWindow.client;
                order.WorkList = GetWorks(addOrderWindow.workList).ToList();
                if(WebRequestHelper.Put.Send(order, "editOrder"))
                {
                    int index = Orders.IndexOf(Orders.FirstOrDefault(x => x.Id == order.Id));
                    Orders[index] = order;
                }
            }
        }
    }

}


using Dragablz.Themes;
//using ServiceCenterApi.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Логика взаимодействия для AddOrderWindow.xaml
    /// </summary>
    public partial class AddOrderWindow : Window
    {
        public ObservableCollection<Client> Clients { get; set; }
        public Order Order { get; set; }
        Grid oldGrid;
        TextBox oldTextBox;
        TextBox oldPriceTextBox;
        Border oldBorder;
        Label oldLabel;
        public Client client;
        public List<Work> workList;
        public AddOrderWindow()
        {
            InitializeComponent();
            oldGrid = (Grid)workListStackPanel.Children[0];
            oldTextBox = (TextBox)oldGrid.Children[0];
            oldPriceTextBox = (TextBox)oldGrid.Children[1];
            oldBorder = (Border)oldGrid.Children[2];
            oldLabel = (Label)oldBorder.Child;
            workList = new List<Work>();
        }

        public AddOrderWindow(ObservableCollection<Client> clients)
        {
            InitializeComponent();
            oldGrid = (Grid)workListStackPanel.Children[0];
            oldTextBox = (TextBox)oldGrid.Children[0];
            oldPriceTextBox = (TextBox)oldGrid.Children[1];
            oldBorder = (Border)oldGrid.Children[2];
            oldLabel = (Label)oldBorder.Child;
            Clients = clients;
            workList = new List<Work>();
        }

        public AddOrderWindow(ObservableCollection<Client> clients, Order order)
        {
            InitializeComponent();
            oldGrid = (Grid)workListStackPanel.Children[0];
            oldTextBox = (TextBox)oldGrid.Children[0];
            oldPriceTextBox = (TextBox)oldGrid.Children[1];
            oldBorder = (Border)oldGrid.Children[2];
            oldLabel = (Label)oldBorder.Child;
            Clients = clients;
            Order = order;
            workList = new List<Work>();
            okBtn.Content = "Применить";

            int index = Clients.IndexOf(Clients.FirstOrDefault(x => x.Id == order.Client.Id));
            addOrderClientsListView.SelectedIndex = index;
            for(int i = 0; i < order.WorkList.Count; i++)
            {
                ((TextBox)((Grid)workListStackPanel.Children[i]).Children[0]).Text = order.WorkList[i].Name;
                ((TextBox)((Grid)workListStackPanel.Children[i]).Children[1]).Text = order.WorkList[i].Price.ToString();
                if (i == order.WorkList.Count - 1)
                    continue;
                AddWorkTextBox();
            }
            
        }

        //Удаляет строку из списка работ
        private void deleteWorkLabel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Border par = (Border)((Label)sender).Parent;
            Grid par2 = (Grid)par.Parent;
            workListStackPanel.Children.Remove(par2);
            int index = 0;
            foreach (var el in workListStackPanel.Children)
            {
                if (el != null)
                    if (el.GetType() == typeof(Grid))
                    {
                        index++;
                        int tag = 0;
                        int.TryParse(((TextBox)((Grid)el).Children[0]).Tag.ToString(), out tag);
                        if (tag != index)
                        {
                            ((TextBox)((Grid)el).Children[0]).Tag = index.ToString();
                            ((TextBox)((Grid)el).Children[0]).Name = $"workTextBox{index}";
                            ((Grid)el).Name = $"workGrid{index}";
                            ((Label)((Border)((Grid)el).Children[1]).Child).Name = $"deleteWorkLabel{index}";
                        }
                    }
            }
        }

        private void addWorkLabel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            AddWorkTextBox();

        }

        //Добавляет строку в список работ
        void AddWorkTextBox()
        {
            int index = workListStackPanel.Children.Count + 1;

            Grid grid = new Grid();
            grid.Name = $"workGrid{index}";
            ColumnDefinition column1 = new ColumnDefinition();
            ColumnDefinition column2 = new ColumnDefinition();
            ColumnDefinition column3 = new ColumnDefinition();
            column1.Width = oldGrid.ColumnDefinitions[0].Width;
            column2.Width = oldGrid.ColumnDefinitions[1].Width;
            column3.Width = oldGrid.ColumnDefinitions[2].Width;
            grid.ColumnDefinitions.Add(column1);
            grid.ColumnDefinitions.Add(column2);
            grid.ColumnDefinitions.Add(column3);

            TextBox textBox = new TextBox();
            textBox.Name = $"workTextBox{index}";
            textBox.Tag = index.ToString();
            textBox.Style = oldTextBox.Style;
            textBox.Margin = new Thickness(5);
            textBox.MaxWidth = oldTextBox.MaxWidth;
            textBox.SetValue(Grid.ColumnProperty, 0);
            textBox.SetValue(MaterialDesignThemes.Wpf.HintAssist.HintProperty, oldTextBox.GetValue(MaterialDesignThemes.Wpf.HintAssist.HintProperty));

            TextBox priceTextBox = new TextBox();
            priceTextBox.Name = $"workPriceTextBox{index}";
            priceTextBox.Tag = index.ToString();
            priceTextBox.Text = "0";
            priceTextBox.Style = oldPriceTextBox.Style;
            priceTextBox.Margin = new Thickness(5);
            priceTextBox.MaxWidth = oldPriceTextBox.MaxWidth;
            priceTextBox.SetValue(Grid.ColumnProperty, 1);
            priceTextBox.SetValue(MaterialDesignThemes.Wpf.HintAssist.HintProperty, oldPriceTextBox.GetValue(MaterialDesignThemes.Wpf.HintAssist.HintProperty));
            priceTextBox.TextChanged += workPriceTextBox1_TextChanged;

            Border border = new Border();
            border.BorderThickness = oldBorder.BorderThickness;
            border.Padding = oldBorder.Padding;
            border.VerticalAlignment = oldBorder.VerticalAlignment;
            border.HorizontalAlignment = oldBorder.HorizontalAlignment;
            border.CornerRadius = oldBorder.CornerRadius;
            border.Margin = oldBorder.Margin;
            border.BorderBrush = oldBorder.BorderBrush;

            border.SetValue(Grid.ColumnProperty, 2);

            Label label = new Label();
            label.Name = $"deleteWorkLabel{index}";
            label.Content = oldLabel.Content;
            label.Foreground = oldLabel.Foreground;
            label.Cursor = oldLabel.Cursor;
            label.Margin = oldLabel.Margin;
            label.Padding = oldLabel.Padding;
            label.FontSize = oldLabel.FontSize;
            label.HorizontalAlignment = oldLabel.HorizontalAlignment;
            label.VerticalAlignment = oldLabel.VerticalAlignment;
            label.MouseLeftButtonUp += deleteWorkLabel_MouseLeftButtonUp;

            border.Child = label;

            grid.Children.Add(textBox);
            grid.Children.Add(priceTextBox);
            grid.Children.Add(border);
            workListStackPanel.Children.Add(grid);
        }

        private async void Accept_Click(object sender, RoutedEventArgs e)
        {
            if (addOrderClientsListView.SelectedItem != null)
            {
                bool workListIsFilling = true;
                int index = -1;
                foreach (Grid el in workListStackPanel.Children)
                {
                    index++;
                    if (((TextBox)el.Children[0]).Text.Length < 1)
                    {
                        workListIsFilling = false;
                        break;
                    }
                }
                if (workListIsFilling)
                {
                    client = addOrderClientsListView.SelectedItem as Client;
                    foreach (Grid el in workListStackPanel.Children)
                    {
                        workList.Add(new Work
                        {
                            Name = ((TextBox)el.Children[0]).Text,
                            Price = decimal.Parse(((TextBox)el.Children[1]).Text)
                        });
                    }
                    DialogResult = true;
                }
                else
                {
                    for (int i = 0; i < 5; i++)
                    {
                        ((TextBox)((Grid)workListStackPanel.Children[index]).Children[0]).Background = new SolidColorBrush() { Color = Color.FromRgb(255, 217, 217) };
                        await Task.Delay(50);
                        ((TextBox)((Grid)workListStackPanel.Children[index]).Children[0]).Background = Brushes.White;
                        await Task.Delay(50);
                    }
                }
            }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    clientsListViewBorder.BorderBrush = Brushes.Red;
                    await Task.Delay(50);
                    clientsListViewBorder.BorderBrush = oldBorder.BorderBrush;
                    await Task.Delay(50);
                }
            }
        }

        private void workPriceTextBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            decimal num = 0;
            if (!decimal.TryParse(((TextBox)sender).Text, out num))
            {
                if (((TextBox)sender).Text.Last() == '.')
                {
                    ((TextBox)sender).Text = ((TextBox)sender).Text.Replace('.', ',');
                }
                else
                {
                    try
                    {
                        ((TextBox)sender).Text = ((TextBox)sender).Text.TrimEnd(((TextBox)sender).Text.Last());
                    }
                    catch { }
                }
            }
            ((TextBox)sender).CaretIndex = ((TextBox)sender).Text.Length;
        }
    }
}

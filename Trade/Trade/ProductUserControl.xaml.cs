using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.SqlClient;
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

namespace Trade
{
    public partial class ProductUserControl : UserControl, INotifyPropertyChanged
    {
        public ProductUserControl()
        {
            InitializeComponent();
            Products = new ObservableCollection<Product>();
            LoadProducts();
        }
        private ObservableCollection<Product> _products;
        public ObservableCollection<Product> Products
        {
            get { return _products; }
            set
            {
                if (_products != value)
                {
                    _products = value;
                    OnPropertyChanged(nameof(Products));
                }
            }
        }
        public Product SelectedProduct { get; set; }
        public void LoadProducts()
        {
            ProductContext myDbContext = new ProductContext();
            using (SqlConnection connection = new SqlConnection(myDbContext.connectionString))
            {
                connection.Open();
                string query = "SELECT Name, Cost, Description, Image, QuantityInStock, Manufacturer FROM Product";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        List<Product> products = new List<Product>();

                        while (reader.Read())
                        {
                            Product product = new Product
                            {
                                Name = reader.IsDBNull(0) ? string.Empty : reader.GetString(0),
                                Cost = reader.IsDBNull(1) ? 0 : reader.GetDecimal(1),
                                Description = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                                Image = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                                QuantityInStock = reader.IsDBNull(4) ? 0 : reader.GetInt32(4),
                                Manufacturer = reader.IsDBNull(5) ? string.Empty : reader.GetString(5)
                            };

                            products.Add(product);
                        }
                        Products = new ObservableCollection<Product>(products);
                    }
                }
            }
        }
        public void SearchTextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                string searchText = textBox.Text.ToLower();
                var filteredProducts = _products.Where(p => p.Name.ToLower().Contains(searchText) ||
                                                           p.Description.ToLower().Contains(searchText) ||
                                                           p.Manufacturer.ToLower().Contains(searchText));
                Products = new ObservableCollection<Product>(filteredProducts);
            }
        }
        public void SelectedSortIndex(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                ComboBoxItem selectedItem = comboBox.SelectedItem as ComboBoxItem;
                if (selectedItem != null)
                {
                    string selectedSortOption = selectedItem.Content.ToString();
                    if (selectedSortOption == "По Возрастанию")
                    {
                        SortAscending();
                    }
                    else if (selectedSortOption == "По Убыванию")
                    {
                        SortDescending();
                    }
                    else if (selectedSortOption == "Нет сортировки")
                    {
                        ResetSort();
                    }
                }
            }
        }
        private void SortAscending()
        {
            var sortedProducts = _products.OrderBy(p => p.Cost);
            Products = new ObservableCollection<Product>(sortedProducts);
        }
        private void ResetSort()
        {
            var sortedProducts = _products.OrderBy(p => p.Cost);
            Products = new ObservableCollection<Product>(sortedProducts);
        }
        private void SortDescending()
        {
            var sortedProducts = _products.OrderByDescending(p => p.Cost);
            Products = new ObservableCollection<Product>(sortedProducts);
        }

        public void FilterByManufacturer(string Manufacturer)
        {
            var filteredProducts = _products.Where(p => p.Manufacturer == Manufacturer);
            Products = new ObservableCollection<Product>(filteredProducts);
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

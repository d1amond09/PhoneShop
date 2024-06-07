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
using PhoneShop.BLL.Implementations;
using PhoneShop.BLL.Interfaces;
using PhoneShop.Domain.Models;
using PhoneShop.Domain.ViewModels;
using PhoneShop.Views.AddPages;

namespace PhoneShop.Views
{
	public partial class SaleView : Page
	{
		private readonly IService<Sale, SaleViewModel> _saleService;
		private readonly IService<Customer> _customerService;
		private readonly IService<Product, ProductViewModel> _productService;

		private IGetService<SaleViewModel> SaleGetService => _saleService;

		public Visibility VisibilityRemove 
		{
			get { return removeBtn.Visibility; }
			set { removeBtn.Visibility = value; }
		}

		public SaleView(IService<Sale, SaleViewModel> saleService, IService<Customer> customerService, IService<Product, ProductViewModel> productService)
		{
			InitializeComponent();
			_saleService = saleService;
			_customerService = customerService;
			_productService = productService;
			saleDataGrid.ItemsSource = SaleGetService.GetAll();
		}

		private void RemoveButton_Click(object sender, RoutedEventArgs e)
		{
			if (MessageBox.Show("Вы уверены, что хотите удалить эту запись? \n" +
								"Запись также удалится в базе данных.",
								"Удаление записи",
								MessageBoxButton.YesNo,
								MessageBoxImage.Question) == MessageBoxResult.Yes)
			{
				if(saleDataGrid.SelectedItem is SaleViewModel saleVM)
				{
					Customer customer = _customerService.Get(saleVM.CustomerFullName).First();
					Sale sale = new()
					{
						Product = new() { Name = saleVM.ProductName },
						Customer = customer,
						Date = DateTime.Parse(saleVM.Date),
						Price = double.Parse(saleVM.Price),	
					};

					_saleService.Remove(sale);
					saleDataGrid.ItemsSource = SaleGetService.GetAll();
				}
			}
		}

		private void AddButton_Click(object sender, RoutedEventArgs e)
		{
			AddFrame.Content = new AddSalePage(_saleService, _customerService, _productService);
			saleDataGrid.ItemsSource = SaleGetService.GetAll();
		}

		private void SearchBy_TextChanged(object sender, TextChangedEventArgs e)
		{
			if(txtSearch.Text != null && txtSearch.Text != string.Empty)
			{
				saleDataGrid.ItemsSource = SaleGetService.Get(txtSearch.Text);
			}
			else
			{
				saleDataGrid.ItemsSource = SaleGetService.GetAll();
			}
		}
	}
}

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
using PhoneShop.BLL.Interfaces;
using PhoneShop.Domain.Models;
using PhoneShop.Domain.ViewModels;
using PhoneShop.Views.AddPages;

namespace PhoneShop.Views
{
	public partial class ProductView : Page
	{
		private readonly IProductService _productService;
		private readonly IGetService<Category> _categoryService;
		private readonly IGetModelService<Phone> _phoneService;

		private IGetService<ProductViewModel> ProductGetService => _productService;

		public ProductView(IProductService productService, IGetService<Category> categoryService, IGetModelService<Phone> phoneService)
		{
			InitializeComponent();
			_productService = productService;
			_categoryService = categoryService;
			_phoneService = phoneService;

			saleDataGrid.ItemsSource = ProductGetService.GetAll();
		}
		private void RemoveButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (MessageBox.Show("Вы уверены, что хотите удалить эту запись? \n" +
									"Запись также удалится в базе данных.",
									"Удаление записи",
									MessageBoxButton.YesNo,
									MessageBoxImage.Question) == MessageBoxResult.Yes &&
					saleDataGrid.SelectedItem is ProductViewModel productVM)
				{
					Product product = new()
					{
						Name = productVM.Name,
						Category = new() { Name = productVM.Category },
						Phone = new() { Name = productVM.Phone },
						Price = productVM.Price,
					};
					_productService.Remove(product);
					saleDataGrid.ItemsSource = ProductGetService.GetAll();
				}
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
		private void AddButton_Click(object sender, RoutedEventArgs e)
		{
			AddFrame.Content = new AddProductPage(_productService, _categoryService, _phoneService);
			saleDataGrid.ItemsSource = ProductGetService.GetAll();
		}

		private void SearchBy_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (txtSearch.Text != null && txtSearch.Text != string.Empty)
			{
				saleDataGrid.ItemsSource = ProductGetService.Get(txtSearch.Text);
			}
			else
			{
				saleDataGrid.ItemsSource = ProductGetService.GetAll();
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Globalization;
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
using PhoneShop.Domain.Models;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using PhoneShop.BLL.Interfaces;
using PhoneShop.Domain.ViewModels;

namespace PhoneShop.Views.AddPages;

public partial class AddSalePage : Page
{
	private readonly IAddService<Sale> _saleService;
	private readonly IService<Customer> _customerService;
	private readonly IService<Product> _productService;
	private IGetModelService<Product> ProductGetService => _productService;
	public AddSalePage(IAddService<Sale> saleService, IService<Customer> customerService, IService<Product> productService)
	{
		_saleService = saleService;
		_customerService = customerService;
		_productService = productService;
		InitializeComponent();
		comboBoxProduct.ItemsSource = ProductGetService.GetAll().Select(p => p.Name);
		date.SelectedDate = DateTime.Now;
	}

	private void CancelButton_Click(object sender, RoutedEventArgs e)
	{
		Content = null;
	}

	private void SaveButton_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			txtBlockProduct.Text = string.Empty;
			txtBlockCustomer.Text = string.Empty;
			txtBlockDate.Text = string.Empty;

			string customerFullName = txtCustomer.Text;
			DateTime? saleDateTime = date.SelectedDate;
			var productName = comboBoxProduct.SelectedItem;

			if (saleDateTime == null)
			{
				txtBlockDate.Text = "Не указана дата продажи!";
				return;
			}

			if (productName == null)
			{
				txtBlockProduct.Text = "Не указан товар для продажи!";
				return;
			}

			Customer customer = _customerService.Get(customerFullName).First();
			if(customer.Id == 0)
			{
				_customerService.Add(customer);
				customer.Id = _customerService.GetAll().Last().Id;
			}

			foreach (Product product in ProductGetService.GetAll())
			{
				if (product.Name == productName.ToString())
				{
					_productService.Remove(product);
					DateTime saleDT = (DateTime)saleDateTime;
					_saleService.Add(new(0, product, customer, saleDT));
					MessageBox.Show("Новая продажа успешно добавлена!");
					Content = null;
				}
			}
		}
		catch(Exception ex)
		{
			txtBlockCustomer.Text = ex.Message;
		}
	}
}

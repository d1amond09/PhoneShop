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
using PhoneShop.BLL.Interfaces;
using PhoneShop.Domain.Models;
using PhoneShop.Domain.ViewModels;

namespace PhoneShop.Views.AddPages;

public partial class AddProductPage : Page
{
	private readonly IAddService<Product> _productService;
	private readonly IGetService<Category> _categoryService;
	private readonly IGetModelService<Phone> _phoneService;

	public AddProductPage(IAddService<Product> productService, IGetService<Category> categoryService, IGetModelService<Phone> phoneService)
	{
		_categoryService = categoryService;
		_productService = productService;
		_phoneService = phoneService;
		InitializeComponent();
		comboBoxPhone.ItemsSource = _phoneService.GetAll().Select(r => r.Name);
		comboBoxCategory.ItemsSource = categoryService.GetAll().Select(r => r.Name);
	}

	private void CancelButton_Click(object sender, RoutedEventArgs e)
	{
		Content = null;
	}

	private void SaveButton_Click(object sender, RoutedEventArgs e)
	{
		txtBlockName.Text = string.Empty;
		txtBlockCategory.Text = string.Empty;
		txtBlockPhone.Text = string.Empty;

		string name = txtName.Text;
		string priceStr = txtPrice.Text;
		var categoryName = comboBoxCategory.SelectedItem;
		var phoneName = comboBoxPhone.SelectedItem;

		if (categoryName == null)
		{
			txtBlockCategory.Text = "Не указана категория товара!";
			return;
		}

		if (phoneName == null)
		{
			txtBlockPhone.Text = "Не указано название телефона!";
			return;
		}

		if(!name.Contains(phoneName.ToString()!))
		{
			txtBlockName.Text = "Наименование товара должно содержать название телефона!";
			return;
		}

		if (double.TryParse(priceStr.Replace(',', '.'), CultureInfo.InvariantCulture, out double price))
		{
			foreach (Category category in _categoryService.GetAll())
			{
				foreach (Phone phone in _phoneService.GetAll())
				{
					if (category.Name == categoryName.ToString() && 
						phone.Name == phoneName.ToString())
					{
						_productService.Add(new(0, name, phone, category, price));
						MessageBox.Show("Новый товар успешно добавлен!");
						Content = null;
					}
				}
			}
		}
		else
		{
			txtBlockPrice.Text = "Неверный формат ввода цены!";
			return;
		}


	}
}

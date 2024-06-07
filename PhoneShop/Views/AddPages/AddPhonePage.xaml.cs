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

namespace PhoneShop.Views.AddPages;

public partial class AddPhonePage : Page
{
	private readonly IService<Phone, PhoneViewModel> _phoneService;
	private readonly IGetService<Producer> _producerService;
	private readonly IGetService<PhoneType> _phoneTypeService;

	public AddPhonePage(IService<Phone, PhoneViewModel> phoneService, IGetService<Producer> producerService, IGetService<PhoneType> phoneTypeService)
	{
		_phoneService = phoneService;
		_producerService = producerService;
		_phoneTypeService = phoneTypeService;
		InitializeComponent();
		comboBoxProducer.ItemsSource = _producerService.GetAll().Select(r => r.Name);
		comboBoxType.ItemsSource = _phoneTypeService.GetAll().Select(r => r.Name);
	}

	private void CancelButton_Click(object sender, RoutedEventArgs e)
	{
		Content = null;
	}

	private void SaveButton_Click(object sender, RoutedEventArgs e)
	{
		txtBlockName.Text = string.Empty;
		txtBlockProducer.Text = string.Empty;
		txtBlockType.Text = string.Empty;

		string name = txtName.Text;
		object? producerName = comboBoxProducer.SelectedItem;
		object? phoneType = comboBoxType.SelectedItem;

		if (name.Length <= 2)
		{
			txtBlockName.Text = "Длина логина должна быть не менее 3 символов!";
			return;
		}
		if (producerName == null)
		{
			txtBlockProducer.Text = "Не указан доступ для пользователя!";
			return;
		}
		if (phoneType == null)
		{
			txtBlockType.Text = "Не указан доступ для пользователя!";
			return;
		}

		foreach (Producer producer in _producerService.GetAll())
		{
			foreach (PhoneType type in _phoneTypeService.GetAll())
			{
				if (producer.Name == producerName.ToString() && type.Name == phoneType.ToString())
				{
					_phoneService.Add(new(0, name, producer, type));
					MessageBox.Show("Новый телефон успешно добавлен!");
					Content = null;
				}
			}
		}
	}
}

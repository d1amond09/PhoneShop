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
	public partial class PhoneView : Page
	{
		private readonly IService<Phone, PhoneViewModel> _phoneService;
		private readonly IGetService<Producer> _producerService;
		private readonly IGetService<PhoneType> _phoneTypeService;
		private IGetService<PhoneViewModel> PhoneGetService => _phoneService;

		public PhoneView(IService<Phone, PhoneViewModel> phoneService, IGetService<Producer> producerService, IGetService<PhoneType> phoneTypeService)
		{
			InitializeComponent();
			_phoneService = phoneService;
			_producerService = producerService;
			_phoneTypeService = phoneTypeService;

			saleDataGrid.ItemsSource = PhoneGetService.GetAll();
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
					saleDataGrid.SelectedItem is PhoneViewModel phoneVM)
				{
					Phone phone = new()
					{
						Name = phoneVM.Name,
						Producer = new() { Name = phoneVM.Producer },
						Type = new() { Name = phoneVM.Type }
					};
					_phoneService.Remove(phone);
				}
				saleDataGrid.ItemsSource = PhoneGetService.GetAll();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
		private void AddButton_Click(object sender, RoutedEventArgs e)
		{
			AddFrame.Content = new AddPhonePage(_phoneService, _producerService, _phoneTypeService);
			saleDataGrid.ItemsSource = PhoneGetService.GetAll();
		}

		private void SearchBy_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (txtSearch.Text != null && txtSearch.Text != string.Empty)
			{
				saleDataGrid.ItemsSource = PhoneGetService.Get(txtSearch.Text);
			}
			else
			{
				saleDataGrid.ItemsSource = PhoneGetService.GetAll();
			}
		}
	}
}

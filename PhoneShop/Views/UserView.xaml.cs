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
using PhoneShop.Domain.Models.Access;
using PhoneShop.Domain.ViewModels;
using PhoneShop.Views.AddPages;

namespace PhoneShop.Views
{
	public partial class UserView : Page
	{
		private readonly IService<User, UserViewModel> _userService;
		private readonly IGetService<Role> _roleService;
		private IGetService<UserViewModel> UserGetService => _userService;
		public UserView(IService<User, UserViewModel> userService, IGetService<Role> roleService)
		{
			InitializeComponent();
			_userService = userService;
			_roleService = roleService;
			saleDataGrid.ItemsSource = UserGetService.GetAll();
		}

		private void RemoveButton_Click(object sender, RoutedEventArgs e)
		{
			if (MessageBox.Show("Вы уверены, что хотите удалить эту запись? \n" +
								"Запись также удалится в базе данных.",
								"Удаление записи",
								MessageBoxButton.YesNo,
								MessageBoxImage.Question) == MessageBoxResult.Yes)
			{
				if (saleDataGrid.SelectedItem is UserViewModel userVM)
				{
					Role role = _roleService.Get(userVM.Role).First();
					User user = new()
					{
						Login = userVM.Login,
						Password = userVM.Password,
						Role = role
					};
					_userService.Remove(user);
					saleDataGrid.ItemsSource = UserGetService.GetAll();
				}
			}
		}
		private void AddButton_Click(object sender, RoutedEventArgs e)
		{
			AddFrame.Content = new AddUserPage(_roleService, _userService);
			saleDataGrid.ItemsSource = UserGetService.GetAll();
		}

		private void SearchBy_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (txtSearch.Text != null && txtSearch.Text != string.Empty)
			{
				saleDataGrid.ItemsSource = UserGetService.Get(txtSearch.Text);
			}
			else
			{
				saleDataGrid.ItemsSource = UserGetService.GetAll();
			}
		}
	}
}


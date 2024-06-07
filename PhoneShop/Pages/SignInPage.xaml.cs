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
using PhoneShop.Domain.ViewModels;

namespace PhoneShop.Pages
{
	/// <summary>
	/// Логика взаимодействия для SignInPage.xaml
	/// </summary>
	public partial class SignInPage : Page
	{
		private readonly MainWindow _window;
		private readonly IGetService<UserViewModel> _userService;

		public SignInPage(MainWindow window, IGetService<UserViewModel> userService)
		{
			InitializeComponent();
			_window = window;
			_userService = userService;
		}

		private void BtnMinimize_Click(object sender, RoutedEventArgs e)
		{
			_window.WindowState = WindowState.Minimized;
        }

		private void BtnClose_Click(object sender, RoutedEventArgs e)
		{
			_window.Close();
        }

		private void BtnLogin_Click(object sender, RoutedEventArgs e)
		{
			txtBlockError.Text = string.Empty;
			bool isNotLogin = true;
			string login = txtLogin.Text;
			string password = txtPassword.Password;

			if (login.Length <= 0)
			{
				txtBlockError.Text = "Логин пользователя не введен";
				return;
			}
			if (password.Length <= 0)
			{
				txtBlockError.Text = "Пароль пользователя не введен";
				return;
			}

			foreach(UserViewModel user in _userService.GetAll())
			{
				if(user.Login == login && user.Password == password) 
				{
					_window.RoleName = user.Role;
					isNotLogin = false;
					Content = null;
				}
			}

			if(isNotLogin)
			{
				txtBlockError.Text = "Логин или пароль введен неверно!";
			}

			_window.UpdateVisibility();
		}

		private void TxtLogin_TextChanged(object sender, TextChangedEventArgs e)
		{
			txtBlockError.Text = string.Empty;
		}

		private void TxtPassword_PasswordChanged(object sender, RoutedEventArgs e)
		{
			txtBlockError.Text = string.Empty;
		}
	}
}

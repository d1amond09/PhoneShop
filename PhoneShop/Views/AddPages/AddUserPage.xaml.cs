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

namespace PhoneShop.Views.AddPages;

public partial class AddUserPage : Page
{
	private readonly IGetService<Role> _roleService;
	private readonly IService<User, UserViewModel> _userService;

	private IGetModelService<User> UserGetService => _userService;

	public AddUserPage(IGetService<Role> roleService, IService<User, UserViewModel> userService)
    {
		_roleService = roleService;
		_userService = userService;
        InitializeComponent();
		comboBoxRole.ItemsSource = _roleService.GetAll().Select(r => r.Name);
	}

	private void CancelButton_Click(object sender, RoutedEventArgs e)
	{
		Content = null;
	}

	private void SaveButton_Click(object sender, RoutedEventArgs e)
	{
		try
		{

			txtBlockAccess.Text = string.Empty;
			txtBlockLogin.Text = string.Empty;
			txtBlockPassword.Text = string.Empty;

			string login = txtLogin.Text;
			string password = txtPassword.Text;
			object? roleName = comboBoxRole.SelectedItem;

			if(roleName == null)
			{
				txtBlockAccess.Text = "Не указан доступ для пользователя!";
				return;
			}

			if (login.Length <= 3 || login.Contains(' '))
			{
				txtBlockLogin.Text = "Логин должен быть не менее 3 символов и не содержать пробелов!";
				return;
			}

			if (password.Length < 8 || login.Contains(' '))
			{
				txtBlockPassword.Text = "Пароль должен быть не менее 8 символов и не содержать пробелов!";
				return;
			}

			if(UserGetService.GetAll().Any(x => x.Login == login))
			{
				txtBlockLogin.Text = "Пользователь с таким логином уже существует!";
				return;
			}


			foreach(var role in _roleService.GetAll())
			{
				if (role.Name == roleName.ToString())
				{
					_userService.Add(new(0, role, login, password));
					MessageBox.Show("Новый пользователь успешно добавлен!");
					Content = null;
				}
			}
		}
		catch(Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}
}

using System.Diagnostics;
using System.IO;
using System.Text;
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
using PhoneShop.DAL;
using PhoneShop.DAL.Interfaces;
using PhoneShop.DAL.Repositories;
using PhoneShop.Domain.Models;
using PhoneShop.Domain.Models.Access;
using PhoneShop.Domain.ViewModels;
using PhoneShop.Pages;
using PhoneShop.Views;

namespace PhoneShop
{
	public partial class MainWindow : Window
	{
		public string RoleName { get; set; }

		private readonly ISaleService _saleService;
		private readonly IProductService _productService;
		private readonly IService<Phone, PhoneViewModel> _phoneService;
		private readonly IService<User, UserViewModel> _userService;

		private readonly IService<Customer> _customerService;

		private readonly IGetService<Role> _roleService;
		private readonly IGetService<PhoneType> _phoneTypeService;
		private readonly IGetService<Producer> _producerService;
		private readonly IGetService<Category> _categoryService;


		public MainWindow()
		{
			RoleName = "";
			InitializeComponent();

			IDbContext dbContext = new DbContext();
			IRepository<User> userRep = new UserRepository(dbContext);
			_userService = new UserService(userRep);

			PageFrame.Content = new SignInPage(this, _userService);

			IRepository<Sale> saleRep = new SaleRepository(dbContext);
			_saleService = new SaleService(saleRep);

			IRepository<Product> productRep = new ProductRepository(dbContext);
			_productService = new ProductService(productRep);

			IRepository<PhoneType> phoneTypeRep = new PhoneTypeRepository(dbContext);
			_phoneTypeService = new PhoneTypeService(phoneTypeRep);

			IRepository<Producer> producerRep = new ProducerRepository(dbContext);
			_producerService = new ProducerService(producerRep);

			IRepository<Customer> customerRep = new CustomerRepository(dbContext);
			_customerService = new CustomerService(customerRep);

			IRepository<Phone> phoneRep = new PhoneRepository(dbContext);
			_phoneService = new PhoneService(phoneRep);

			IRepository<Category> categoryRep = new CategoryRepository(dbContext);
			_categoryService = new CategoryService(categoryRep);

			IRepository<Role> roleRep = new RoleRepository(dbContext);
			_roleService = new RoleService(roleRep);
		}

		private void CloseButton_Click(object sender, RoutedEventArgs e)
		{
			PageFrame.Content = new SignInPage(this, _userService);
			ViewFrame.Content = null;
		}

		private void Border_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
			{
				DragMove();
			}
		}

		private bool _isMaximized = false;

		private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ClickCount == 2)
			{
				if (_isMaximized)
				{
					WindowState = WindowState.Normal;
					Width = 1250;
					Height = 720;

					_isMaximized = false;
				}
				else
				{
					WindowState = WindowState.Maximized;

					_isMaximized = true;
				}
			}
		}

		private void UsersButton_Click(object sender, RoutedEventArgs e)
		{
			ViewFrame.Content = new UserView(_userService, _roleService);
		}

		private void ProductsButton_Click(object sender, RoutedEventArgs e)
		{
			ViewFrame.Content = new ProductView(_productService, _categoryService, _phoneService);
		}

		private void SalesButton_Click(object sender, RoutedEventArgs e)
		{
			Visibility visibilityRemove = RoleName == "Продавец" ? Visibility.Hidden : Visibility.Visible;
			ViewFrame.Content = new SaleView(_saleService, _customerService, _productService)
			{
				VisibilityRemove = visibilityRemove
			};
		}

		private void PhonesButton_Click(object sender, RoutedEventArgs e)
		{
			ViewFrame.Content = new PhoneView(_phoneService, _producerService, _phoneTypeService);
		}

		public void UpdateVisibility()
		{
			if (RoleName == "Администратор")
			{
				btnUsers.Visibility = Visibility.Visible;
				btnPhones.Visibility = Visibility.Visible;
				btnProducts.Visibility = Visibility.Visible;
				btnSales.Visibility = Visibility.Visible;
				report1.Visibility = Visibility.Visible;
				report2.Visibility = Visibility.Visible;
				report3.Visibility = Visibility.Visible;
				report4.Visibility = Visibility.Visible;
			}
			if (RoleName == "Менеджер")
			{
				btnUsers.Visibility = Visibility.Hidden;
				btnPhones.Visibility = Visibility.Hidden;
				btnProducts.Visibility = Visibility.Visible;
				btnSales.Visibility = Visibility.Hidden;
				report1.Visibility = Visibility.Visible;
				report2.Visibility = Visibility.Visible;
				report3.Visibility = Visibility.Visible;
				report4.Visibility = Visibility.Visible;
			}
			if (RoleName == "Продавец")
			{
				btnUsers.Visibility = Visibility.Hidden;
				btnPhones.Visibility = Visibility.Hidden;
				btnProducts.Visibility = Visibility.Hidden;
				btnSales.Visibility = Visibility.Visible;
				report1.Visibility = Visibility.Hidden;
				report2.Visibility = Visibility.Hidden;
				report3.Visibility = Visibility.Hidden;
				report4.Visibility = Visibility.Hidden;
			
			}
		}

		private void ReportRemainingButton_Click(object sender, RoutedEventArgs e)
		{
			ViewFrame.Content = new ReportPage(_productService, @"products.xml");
		}

		private void ReportProducersButton_Click(object sender, RoutedEventArgs e)
		{
			ViewFrame.Content = new ReportPage(_productService, @"producers.xml");
		}

		private void ReportPhoneTypeButton_Click(object sender, RoutedEventArgs e)
		{
			ViewFrame.Content = new DateReportPage(_saleService, @"producers_count_sales.xml");
		}

		private void ReportProfitButton_Click(object sender, RoutedEventArgs e)
		{
			ViewFrame.Content = new DateReportPage(_saleService, @"type_phones_count_sales_profit.xml");
		}
	}
}
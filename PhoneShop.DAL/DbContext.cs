using System.Configuration;
using MySql.Data.MySqlClient;
using PhoneShop.DAL.DAO;
using PhoneShop.DAL.Interfaces;
using PhoneShop.Domain.Models;
using PhoneShop.Domain.Models.Access;

namespace PhoneShop.DAL;

public class DbContext : IDbContext
{
	private readonly MySqlConnection _connection;

	public IModelDAO<Producer> Producers { get; set; }
	public IModelDAO<Role> Roles { get; set; }
	public IModelDAO<User> Users { get; set; }
	public IModelDAO<Phone> Phones { get; set; }
	public IModelDAO<PhoneType> PhoneTypes { get; set; }
	public IModelDAO<Category> Categories { get; set; }
	public IModelDAO<Product> Products { get; set; }
	public IModelDAO<Product> SoldProducts { get; set; }
	public IModelDAO<Sale> Sales { get; set; }
	public IModelDAO<Customer> Customers { get; set; }

	public DbContext()
	{
		string dbSettings = ConfigurationManager.ConnectionStrings["PhoneShopDb"].ConnectionString;
		_connection = new MySqlConnection(dbSettings);

		Producers = new ProducerDAO(_connection);
		Roles = new RoleDAO(_connection);
		Users = new UserDAO(_connection, Roles);
		PhoneTypes = new PhoneTypeDAO(_connection);
		Producers = new ProducerDAO(_connection);
		Phones = new PhoneDAO(_connection, PhoneTypes, Producers);
		Categories = new CategoryDAO(_connection);
		Customers = new CustomerDAO(_connection);
		Products = new ProductDAO(_connection, Phones, Categories);
		SoldProducts = new SoldProductDAO(_connection, Phones, Categories);
		Sales = new SaleDAO(_connection, Customers, SoldProducts);
	}
}

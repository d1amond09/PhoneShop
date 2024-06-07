using PhoneShop.DAL.DAO;
using PhoneShop.Domain.Models;
using PhoneShop.Domain.Models.Access;

namespace PhoneShop.DAL.Interfaces;

public interface IDbContext
{
	IModelDAO<Role> Roles { get; }
	IModelDAO<User> Users { get; }
	IModelDAO<Producer> Producers { get; }
	IModelDAO<PhoneType> PhoneTypes { get; }
	IModelDAO<Category> Categories { get; }
	IModelDAO<Phone> Phones { get; }
	IModelDAO<Sale> Sales { get; }
	IModelDAO<Customer> Customers { get; }
	IModelDAO<Product> Products { get; }
	IModelDAO<Product> SoldProducts { get; }
}

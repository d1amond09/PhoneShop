using MySql.Data.MySqlClient;
using PhoneShop.DAL.Interfaces;
using PhoneShop.Domain.Models;

namespace PhoneShop.DAL.Repositories;

public class ProductRepository(IDbContext dbContext) : IRepository<Product>
{
	private readonly IDbContext _dbContext = dbContext;

	public bool Create(Product model)
	{
		_dbContext.Products.Insert(model);
		return true;
	}

	public bool Delete(Product model)
	{
		_dbContext.SoldProducts.Insert(model);
		_dbContext.Products.Delete(model.Id);
		return true;
	}

	public Product Read(int id)
	{
		Product? product = ReadAll().FirstOrDefault(x => x.Id == id);
		ArgumentNullException.ThrowIfNull(product);
		return product;
	}

	public IEnumerable<Product> ReadAll()
		=> _dbContext.Products;

	public bool Update(Product oldModel, Product newModel)
	{
		_dbContext.Products.Update(oldModel.Id, newModel);
		return true;
	}
}
